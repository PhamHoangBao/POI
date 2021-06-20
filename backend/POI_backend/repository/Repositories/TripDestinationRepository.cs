using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using POI.repository.Entities;
using POI.repository.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using POI.repository.Enums;



namespace POI.repository.Repositories
{
    public interface ITripDestinationRepository : IGenericRepository<TripDestination>
    {
        public IQueryable<TripDestination> GetTripsDetinationWithDestination(Guid tripID, bool istracked);

        public int GetCurrentTripDestinationOrder(Guid tripID);

        public TripDestination GetCurrentTripDestination(Guid tripID);

    }
    public class TripDestinationRepository : GenericRepository<TripDestination>, ITripDestinationRepository
    {
        public TripDestinationRepository(POIContext context) : base(context)
        {

        }

        public TripDestination GetCurrentTripDestination(Guid tripID)
        {
            return _context.TripDestinations
                .Where(tripDest => tripDest.TripId.Equals(tripID))
                .OrderByDescending(tripdest => tripdest.Order)
                .FirstOrDefault();

        }

        public int GetCurrentTripDestinationOrder(Guid tripID)
        {
            return _context.TripDestinations.Count(tripdest => tripdest.TripId.Equals(tripID));
        }

        public IQueryable<TripDestination> GetTripsDetinationWithDestination(Guid tripID, bool istracked)
        {
            if (istracked)
            {
                return _context.TripDestinations
                    .Include(tripDest => tripDest.Destination)
                    .Include(tripDest => tripDest.Trip)
                    .Select(tripDest => new TripDestination
                    {
                        TripDestinationId = tripDest.TripDestinationId,
                        Destination = tripDest.Destination,
                        Trip = tripDest.Trip,
                        Order = tripDest.Order,
                        TripId = tripDest.TripId,
                        DestinationId = tripDest.DestinationId,
                        Status = tripDest.Status
                    })
                    .Where(tripDest => tripDest.TripId.Equals(tripID)
                            && tripDest.Status != (int)TripDestinationEnum.Available)
                    .OrderByDescending(tripDest => tripDest.Order);
            }
            else
            {
                return _context.TripDestinations
                    .Include(tripDest => tripDest.Destination)
                    .Include(tripDest => tripDest.Trip)
                    .Select(tripDest => new TripDestination
                    {

                        TripDestinationId = tripDest.TripDestinationId,
                        Destination = tripDest.Destination,
                        Trip = tripDest.Trip,
                        Order = tripDest.Order,
                        TripId = tripDest.TripId,
                        DestinationId = tripDest.DestinationId,
                        Status = tripDest.Status
                    })
                    .Where(tripDest => tripDest.TripId.Equals(tripID)
                            && tripDest.Status == (int)TripDestinationEnum.Available)
                    .AsNoTracking()
                    .OrderByDescending(tripDest => tripDest.Order);

            }

        }
    }
}
