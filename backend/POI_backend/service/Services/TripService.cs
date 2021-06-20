using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using POI.repository.Enums;
using POI.repository.Repositories;
using POI.repository.Entities;


namespace POI.service.Services
{
    public interface ITripService : IGenericService<Trip>
    {
        public Task<CreateEnum> CreateNewTrip(CreateTripViewModel trip);

        public UpdateEnum UpdateTrip(UpdateTripViewModel trip);

        public DeleteEnum ArchiveTrip(Guid id);

        public UpdateEnum FinishTrip(Guid id);
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

        public DeleteEnum ArchiveTrip(Guid id)
        {
            Trip trip = _tripRepository.GetByID(id);
            if (trip == null)
            {
                return DeleteEnum.Failed;
            }
            else
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
        }

        public async Task<CreateEnum> CreateNewTrip(CreateTripViewModel trip)
        {
            var entity = _mapper.Map<Trip>(trip);
            entity.StartTime = DateTime.Now;
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

        public UpdateEnum FinishTrip(Guid id)
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
            else
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
        }

        public UpdateEnum UpdateTrip(UpdateTripViewModel trip)
        {
            if (FirstOrDefault(m => m.TripId.Equals(trip.TripId), false) == null)
            {
                return UpdateEnum.Error;
            }
            else
            {
                var entity = _mapper.Map<Trip>(trip);
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
