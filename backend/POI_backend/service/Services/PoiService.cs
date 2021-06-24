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

namespace POI.service.Services
{
    public interface IPoiService : IGenericService<Poi>
    {
        public Task<CreateEnum> CreateNewPoi(CreatePoiViewModel province, Guid userID);
        public DeleteEnum DeactivatePoi(Guid id);
        public UpdateEnum UpdatePoi(UpdatePoiViewModel province);
        public List<ResponsePoiViewModel> GetPoi(Expression<Func<Poi, bool>> predicate, bool istracked);
        public UpdateEnum ApprovePOI(Guid id);
    }
    public class PoiService : GenericService<Poi>, IPoiService
    {
        private readonly IPoiRepository _poiRepository;
        private readonly IMapper _mapper;

        public PoiService(IPoiRepository poiRepository
                                , IMapper mapper
                                ) : base(poiRepository)
        {
            _mapper = mapper;
            _poiRepository = poiRepository;
        }

        public async Task<CreateEnum> CreateNewPoi(CreatePoiViewModel poi, Guid userID)
        {
            if (await FirstOrDefaultAsync(m => m.Name.Equals(poi.Name), false) != null)
            {
                return CreateEnum.Duplicate;
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
                    return CreateEnum.Success;
                }
                catch
                {
                    return CreateEnum.ErrorInServer;
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
        public List<ResponsePoiViewModel> GetPoi(Expression<Func<Poi, bool>> predicate, bool istracked)
        {
            IQueryable<Poi> pois = _poiRepository.GetPoi(predicate, istracked);
            List<Poi> poiList = pois.ToList();
            List<ResponsePoiViewModel> responses = _mapper.Map<List<ResponsePoiViewModel>>(poiList);
            for (int i = 0; i < responses.Count(); i++)
            {
                var response = responses[i];
                var poi = poiList[i];
                response.User = _mapper.Map<AuthenticatedUserViewModel>(poi.User);
                response.Destination = _mapper.Map<ResponseDestinationViewModel>(poi.Destination);
            }
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

    }
}
