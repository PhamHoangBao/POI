using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using System.Linq;
using System.Linq.Expressions;
using StackExchange.Redis.Extensions.Core.Abstractions;
using POI.repository.Utils;
using NetTopologySuite.Geometries;

namespace POI.service.Services
{
    public interface IPoiService : IGenericService<Poi>
    {
        public Task<Tuple<CreateEnum, Guid>> CreateNewPoi(CreatePoiViewModel province, Guid userID);
        public DeleteEnum DeactivatePoi(Guid id);
        public UpdateEnum UpdatePoi(UpdatePoiViewModel province);
        public Task<List<ResponsePoiViewModel>> GetPoi(Expression<Func<Poi, bool>> predicate, bool istracked);
        public Task<PagedList<ResponsePoiViewModel>> GetPOIWithPaging(Expression<Func<Poi, bool>> predicate, bool istracked, int index, int pageSize);
        public UpdateEnum ApprovePOI(Guid id);
        public Task<CreateEnum> AddUserToPoiInRedis(MyPoint point, Guid userID);

        //public IQueryable<Poi> SortPOI(MyPoint point);
    }
    public class PoiService : GenericService<Poi>, IPoiService
    {
        private readonly IPoiRepository _poiRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCacheClient _redisCacheClient;
        private const int Radius = 20;
        public PoiService(IPoiRepository poiRepository
                                , IMapper mapper
                                , IRedisCacheClient redisCacheClient
                                ) : base(poiRepository)
        {
            _mapper = mapper;
            _poiRepository = poiRepository;
            _redisCacheClient = redisCacheClient;
        }

        private async Task<IDictionary<string, string>> GetDataInRedis()
        {
            var keys = (await _redisCacheClient.GetDbFromConfiguration().SearchKeysAsync("*")).ToList();
            var value = await _redisCacheClient.GetDbFromConfiguration().GetAllAsync<string>(keys);
            return value;
        }

        private int CountUserInPOI(IDictionary<string, string> data, Guid poiID)
        {
            int count = 0;
            foreach (KeyValuePair<string, string> item in data)
            {
                if (item.Value.Equals(poiID.ToString()))
                {
                    count = count + 1;
                }
            }
            return count;
        }

        public async Task<Tuple<CreateEnum, Guid>> CreateNewPoi(CreatePoiViewModel poi, Guid userID)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(poi.Name), false) != null)
            {
                return new Tuple<CreateEnum, Guid>(CreateEnum.Duplicate, Guid.Empty);
            }
            else
            {
                var entity = _mapper.Map<Poi>(poi);
                if (!userID.Equals(Guid.Empty))
                {
                    Console.WriteLine("A");
                    entity.UserId = userID;
                    entity.Status = (int)PoiEnum.Pending;
                }
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    return new Tuple<CreateEnum, Guid>(CreateEnum.Success, entity.PoiId);
                }
                catch
                {
                    return new Tuple<CreateEnum, Guid>(CreateEnum.ErrorInServer, Guid.Empty);
                }
            }
        }
        public DeleteEnum DeactivatePoi(Guid id)
        {
            Poi poi = _poiRepository.GetByID(id);
            if (poi == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                poi.Status = (int)PoiEnum.Inactive;
                try
                {
                    Update(poi);
                    Savechanges();
                    return DeleteEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return DeleteEnum.ErrorInServer;
                }
            }
        }

        public UpdateEnum UpdatePoi(UpdatePoiViewModel poi)
        {
            if (FirstOrDefault(m => m.PoiId.Equals(poi.PoiId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Poi>(poi);
                try
                {
                    Update(entity);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
        }
        public async Task<List<ResponsePoiViewModel>> GetPoi(Expression<Func<Poi, bool>> predicate, bool istracked)
        {
            IQueryable<Poi> pois = _poiRepository.GetPoi(predicate, istracked);
            List<Poi> poiList = pois.ToList();
            List<ResponsePoiViewModel> responses = _mapper.Map<List<ResponsePoiViewModel>>(poiList);
            Console.WriteLine("Response count : " + responses.Count);
            for (int i = 0; i < responses.Count(); i++)
            {
                var response = responses[i];
                var poi = poiList[i];
                var count = 0;
                try
                {
                    Console.WriteLine("Trying to get Count ");
                    var redisData = await GetDataInRedis();
                    count = CountUserInPOI(redisData, response.PoiId);
                    Console.WriteLine("Finish get count :  " + count);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                response.Count = count;
                Console.WriteLine("Finish reading3 ");
                response.User = _mapper.Map<AuthenticatedUserViewModel>(poi.User);
                Console.WriteLine("Finish reading1 ");
                response.Destination = _mapper.Map<ResponseDestinationViewModel>(poi.Destination);
                Console.WriteLine("Finish reading ");
            }
            Console.WriteLine("Finish reading2 ");
            return responses;
        }
        public UpdateEnum ApprovePOI(Guid id)
        {
            Poi poi = _poiRepository.GetByID(id);
            if (poi == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                poi.Status = (int)PoiEnum.Available;
                try
                {
                    Update(poi);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
        }

        public async Task<PagedList<ResponsePoiViewModel>> GetPOIWithPaging(Expression<Func<Poi, bool>> predicate, bool istracked, int index, int pageSize)
        {
            IQueryable<Poi> pois = _poiRepository.GetPoi(predicate, istracked);
            PagedList<Poi> poisPageList = PagedList<Poi>.ToPagedList(pois, index, pageSize);
            PagedList<ResponsePoiViewModel> responses = _mapper.Map<PagedList<ResponsePoiViewModel>>(poisPageList);

            for (int i = 0; i < responses.Count(); i++)
            {
                var response = responses[i];
                var poi = poisPageList[i];
                var count = 0;
                try
                {
                    var redisData = await GetDataInRedis();
                    count = CountUserInPOI(redisData, response.PoiId);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                response.Count = count;
                response.User = _mapper.Map<AuthenticatedUserViewModel>(poi.User);
                response.Destination = _mapper.Map<ResponseDestinationViewModel>(poi.Destination);
            }
            return responses;
        }

        public async Task<CreateEnum> AddUserToPoiInRedis(MyPoint point, Guid userID)
        {
            Poi poi = _poiRepository.GetClosestPOI(point);
            Console.WriteLine("Current Poi : " + poi.Name);
            if (poi != null)
            {
                Point currentUserLocation = new Point(point.Longtitude, point.Latitude);
                Point poiLocation = new Point(poi.Location.Coordinate.X, poi.Location.Coordinate.Y);
                var distance = LocationUtils.HaversineDistance(currentUserLocation, poiLocation, LocationUtils.DistanceUnit.Kilometers);
                if (distance <= Radius)
                {
                    bool isAdded = await _redisCacheClient
                        .GetDbFromConfiguration()
                        .AddAsync<string>(userID.ToString(), poi.PoiId.ToString(), DateTimeOffset.Now.AddDays(5));
                    if (isAdded)
                    {
                        return CreateEnum.Success;
                    }
                    else
                    {
                        return CreateEnum.Error;
                    }
                }
            }
            return CreateEnum.Error;
        }
    }
}
