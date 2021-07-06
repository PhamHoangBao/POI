using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using POI.repository.Repositories;
using System.Linq.Expressions;
using POI.repository.Entities;
using System.Linq;


namespace POI.service.Services
{
    public interface ITripService : IGenericService<Trip>
    {
        public Task<Tuple<CreateEnum, Guid>> CreateNewTrip(CreateTripViewModel trip, Guid userID);
        public UpdateEnum UpdateTrip(UpdateTripViewModel trip, Guid userID);
        public DeleteEnum ArchiveTrip(Guid id, Guid userID);
        public UpdateEnum FinishTrip(Guid id, Guid userID);
        public List<ResponseTripViewModel> GetTrips(Expression<Func<Trip, bool>> predicate, bool istracked);
    }
    public class TripService : GenericService<Trip>, ITripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IMapper _mapper;

        public TripService(ITripRepository tripRepository
                                , IMapper mapper
                                ) : base(tripRepository)
        {
            _mapper = mapper;
            _tripRepository = tripRepository;
        }

        public DeleteEnum ArchiveTrip(Guid id, Guid userID)
        {
            Trip trip = _tripRepository.GetByID(id);
            if (trip == null)
            {
                return DeleteEnum.Failed;
            }
            else if (trip.UserId.Equals(userID))
            {
                trip.Status = (int)TripEnum.ARCHIVE;
                try
                {
                    Update(trip);
                    Savechanges();
                    return DeleteEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return DeleteEnum.ErrorInServer;
                }
            }
            else
            {
                return DeleteEnum.NotOwner;
            }
        }

        public async Task<Tuple<CreateEnum, Guid>> CreateNewTrip(CreateTripViewModel trip, Guid userID)
        {
            //var result = new Tuple<CreateEnum, Guid>();
            var lastestTrip = _tripRepository.GetLatestTrip(userID, false);
            if (lastestTrip != null)
            {
                if (lastestTrip.Status == (int)TripEnum.ONGOING)
                {
                    return new Tuple<CreateEnum, Guid>(CreateEnum.Error, Guid.Empty);
                }
            }
            var entity = _mapper.Map<Trip>(trip);
            entity.StartTime = DateTime.Now;
            entity.UserId = userID;
            try
            {
                await AddAsync(entity);
                await SaveChangesAsync();
                return new Tuple<CreateEnum, Guid>(CreateEnum.Success, entity.TripId);
            }
            catch
            {
                return new Tuple<CreateEnum, Guid>(CreateEnum.ErrorInServer, Guid.Empty);
            }
        }

        public UpdateEnum FinishTrip(Guid id, Guid userID)
        {
            Trip trip = FirstOrDefault(trip => trip.TripId.Equals(id), true);
            if (trip == null)
            {
                return UpdateEnum.Error;
            }
            else if (trip.Status != (int)TripEnum.ONGOING)
            {
                return UpdateEnum.Error;
            }
            else if (trip.UserId.Equals(userID))
            {
                try
                {
                    trip.EndTime = DateTime.Now;
                    trip.Status = (int)TripEnum.FINISHED;
                    Update(trip);
                    Savechanges();
                    return UpdateEnum.Success;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return UpdateEnum.ErrorInServer;
                }
            }
            else
            {
                return UpdateEnum.NotOwner;
            }
        }

        public UpdateEnum UpdateTrip(UpdateTripViewModel trip, Guid userID)
        {
            if (FirstOrDefault(m => m.TripId.Equals(trip.TripId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Trip>(trip);
                entity.UserId = userID;
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

        public List<ResponseTripViewModel> GetTrips(Expression<Func<Trip, bool>> predicate, bool istracked)
        {
            IQueryable<Trip> trips = _tripRepository.GetTrips(predicate, istracked);
            List<Trip> tripList = trips.ToList();
            List<ResponseTripViewModel> responses = _mapper.Map<List<ResponseTripViewModel>>(tripList);
            for (int i = 0; i < responses.Count(); i++)
            {
                var response = responses[i];
                var trip = tripList[i];
                response.User = _mapper.Map<AuthenticatedUserViewModel>(trip.User);

                List<Destination> destinationList = trip.TripDestinations.Select(m => m.Destination).ToList();
                response.Destinations = _mapper.Map<List<ResponseDestinationViewModel>>(destinationList);

                List<ResponseVisitViewModel> responseVisits = new List<ResponseVisitViewModel>();
                var visits = trip.Visits;
                foreach (Visit visit in visits)
                {
                    var responseVisit = _mapper.Map<ResponseVisitViewModel>(visit);
                    responseVisits.Add(responseVisit);
                }
                response.Visits = responseVisits;
            }
            return responses;
        }

    }
}
