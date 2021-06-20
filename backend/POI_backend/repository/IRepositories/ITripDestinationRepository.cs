using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;


namespace POI.repository.IRepositories
{
    public interface ITripDestinationRepository : IGenericRepository<TripDestination>
    {
        public IQueryable<TripDestination> GetTripsDetinationWithDestination(Guid tripID, bool istracked);

        public int GetCurrentTripDestinationOrder(Guid tripID);

        public TripDestination GetCurrentTripDestination(Guid tripID);

    }
}

