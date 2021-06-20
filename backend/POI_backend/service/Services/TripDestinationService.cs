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


namespace POI.service.Services
{
    public interface ITripDestinationService : IGenericService<TripDestination>
    {
        public IQueryable<TripDestination> GetAllTripDetinationsWithDestination(Guid tripID);

        public int GetCurrentTripDestinationOrder(Guid tripID);

        public Task<CreateEnum> CreateNewTripDestination(Guid tripId, Guid destinationID);
    }
    public class TripDestinationService : GenericService<TripDestination>, ITripDestinationService
    {
        private readonly ITripDestinationRepository _tripDestinationRepository;
        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;

        public TripDestinationService(ITripDestinationRepository tripDestinationRepository,
                    IMapper mapper,
                    ITripRepository tripRepository
                    ) : base(tripDestinationRepository)
        {
            _mapper = mapper;
            _tripDestinationRepository = tripDestinationRepository;
            _tripRepository = tripRepository;
        }

        public async Task<CreateEnum> CreateNewTripDestination(Guid tripId, Guid destinationID)
        {
            var trip = await _tripRepository.FirstOrDefaultAsync(m => m.TripId.Equals(tripId), false);
            if (trip == null)
            {
                return CreateEnum.Error;
            }
            else if (trip.Status == (int)TripEnum.FINISHED || trip.Status == (int)TripEnum.ARCHIVE)
            {
                return CreateEnum.Error;
            }
            else
            {
                TripDestination currentTripDest = _tripDestinationRepository.GetCurrentTripDestination(tripId);
                if (currentTripDest != null)
                {
                    if (currentTripDest.DestinationId.Equals(destinationID))
                    {
                        return CreateEnum.Duplicate;
                    }
                    var entity = new TripDestination
                    {
                        TripId = tripId,
                        DestinationId = destinationID,
                        Order = currentTripDest.Order + 1,
                        Status = (int)TripDestinationEnum.Available
                    };
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
                else
                {
                    return CreateEnum.Error;
                }

            }
        }

        public IQueryable<TripDestination> GetAllTripDetinationsWithDestination(Guid tripID)
        {
            return _tripDestinationRepository.GetTripsDetinationWithDestination(tripID, false);
        }

        public int GetCurrentTripDestinationOrder(Guid tripID)
        {
            return _tripDestinationRepository.GetCurrentTripDestinationOrder(tripID);
        }
    }
}
