using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.ResultEnums;
using POI.repository.ViewModels;
using System.Linq;

namespace POI.service.IServices
{
    public interface ITripDestinationService : IGenericService<TripDestination>
    {
        public IQueryable<TripDestination> GetAllTripDetinationsWithDestination(Guid tripID);

        public int GetCurrentTripDestinationOrder(Guid tripID);

        public Task<CreateEnum> CreateNewTripDestination(Guid tripId, Guid destinationID);
    }
}
