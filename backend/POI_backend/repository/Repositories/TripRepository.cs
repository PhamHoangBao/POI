using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;


namespace POI.repository.Repositories
{
    public interface ITripRepository : IGenericRepository<Trip>
    {
        public IQueryable<Trip> GetTrips(Expression<Func<Trip, bool>> predicate, bool istracked);
    }
    public class TripRepository : GenericRepository<Trip>, ITripRepository
    {
        public TripRepository(POIContext context) : base(context)
        {

        }

        public IQueryable<Trip> GetTrips(Expression<Func<Trip, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Trips
                    .Include(trip => trip.User)
                    .Include(trip => trip.TripDestinations)
                    .ThenInclude(tripDest => tripDest.Destination)
                    .Select(trip => new Trip
                    {
                        TripId = trip.TripId,
                        TripName = trip.TripName,
                        Status = trip.Status,
                        StartTime = trip.StartTime,
                        EndTime = trip.EndTime,
                        User = new User
                        {
                            UserId = trip.User.UserId,
                            Username = trip.User.Username,
                            FirstName = trip.User.FirstName,
                            LastName = trip.User.LastName,
                            Email = trip.User.Email,
                            Phone = trip.User.Phone,
                            Status = trip.User.Status,
                            Role = trip.User.Role
                        },
                        TripDestinations = trip.TripDestinations.OrderBy(m => m.Order).ToList()
                    })
                    .Where(predicate);
            }
            else
            {
                return _context.Trips
                    .Include(trip => trip.User)
                    .ThenInclude(user => user.Role)
                    .Include(trip => trip.TripDestinations)
                    .ThenInclude(tripDest => tripDest.Destination)
                    .ThenInclude(destination => destination.Province)
                    .Include(trip => trip.TripDestinations)
                    .ThenInclude(tripDest => tripDest.Destination)
                    .ThenInclude(destination => destination.DestinationType)
                    .Select(trip => new Trip
                    {
                        TripId = trip.TripId,
                        TripName = trip.TripName,
                        Status = trip.Status,
                        StartTime = trip.StartTime,
                        EndTime = trip.EndTime,
                        User = new User
                        {
                            UserId = trip.User.UserId,
                            Username = trip.User.Username,
                            FirstName = trip.User.FirstName,
                            LastName = trip.User.LastName,
                            Email = trip.User.Email,
                            Phone = trip.User.Phone,
                            Status = trip.User.Status,
                            Role = trip.User.Role
                        },
                        TripDestinations = trip.TripDestinations.OrderBy(m => m.Order).ToList()
                    })
                    .Where(predicate)
                    .AsNoTracking();
            }
        }
    }
}
