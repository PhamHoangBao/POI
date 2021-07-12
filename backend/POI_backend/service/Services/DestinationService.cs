using System;
using POI.repository.Repositories;
using POI.repository.Entities;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using POI.repository.Utils;

namespace POI.service.Services
{
    public interface IDestinationService : IGenericService<Destination>
    {
        public Task<Tuple<CreateEnum,Guid>> CreateNewDestination(CreateDestinationViewModel destination);
        public DeleteEnum DeactivateDestination(Guid id);
        public UpdateEnum UpdateDestination(UpdateDestinationViewModel destination);
        public List<ResponseDestinationViewModel> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked);
        public IQueryable<Destination> GetDetinationWithProvince(Guid provinceId);
        public List<ResponseDestinationViewModel> GetDestinationWithRadius(double latitude, double longtitude, double radius);
    }
    public class DestinationService : GenericService<Destination>, IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IMapper _mapper;

        public DestinationService(IDestinationRepository destinationRepository
                                , IMapper mapper
                                ) : base(destinationRepository)
        {
            _mapper = mapper;
            _destinationRepository = destinationRepository;
        }

        public async Task<Tuple<CreateEnum, Guid>> CreateNewDestination(CreateDestinationViewModel destination)
        {
            if (await FirstOrDefaultAsync(m => m.DestinationName.Equals(destination.DestinationName), false) != null)
            {
                return new Tuple<CreateEnum, Guid>(CreateEnum.Duplicate, Guid.Empty);
            }
            else
            {
                var entity = _mapper.Map<Destination>(destination);
                try
                {
                    await AddAsync(entity);
                    await SaveChangesAsync();
                    return new Tuple<CreateEnum, Guid>(CreateEnum.Success, entity.DestinationId);
                }
                catch
                {
                    return new Tuple<CreateEnum, Guid>(CreateEnum.ErrorInServer, Guid.Empty);
                }
            }
        }

        public DeleteEnum DeactivateDestination(Guid id)
        {
            Destination destination = _destinationRepository.GetByID(id);
            if (destination == null)
            {
                return DeleteEnum.Failed;
            }
            else
            {
                destination.Status = (int)DestinationEnum.Disable;
                try
                {
                    Update(destination);
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

        public List<ResponseDestinationViewModel> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked)
        {
            IQueryable<Destination> destinations = _destinationRepository.GetDestination(predicate, istracked);
            List<Destination> destinationList = destinations.ToList();
            List<ResponseDestinationViewModel> responses = _mapper.Map<List<ResponseDestinationViewModel>>(destinationList);
            return responses;
        }

        public List<ResponseDestinationViewModel> GetDestinationWithRadius(double latitude, double longtitude, double radius)
        {
            var destinations = GetDestination(m => m.Status == (int)DestinationEnum.Available, false);
            MyPoint location = new MyPoint(latitude, longtitude);
            var result = destinations.Where(m =>
            {
                var currentLocation = new Point(longtitude, latitude);
                var destinationLocation = new Point(m.Location.Longtitude, m.Location.Latitude);
                double distance = LocationUtils.HaversineDistance(currentLocation, destinationLocation, LocationUtils.DistanceUnit.Kilometers);
                return distance < radius;
            })
            .OrderBy(m => LocationUtils.EuclideDistance(location, m.Location))
            .ToList();
            return result;
        }

        public IQueryable<Destination> GetDetinationWithProvince(Guid provinceId)
        {
            return _destinationRepository.GetDestination(m => m.ProvinceId.Equals(provinceId), false);
        }

        public UpdateEnum UpdateDestination(UpdateDestinationViewModel destination)
        {
            if (FirstOrDefault(m => m.DestinationId.Equals(destination.DestinationId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Destination>(destination);
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

    }
}
