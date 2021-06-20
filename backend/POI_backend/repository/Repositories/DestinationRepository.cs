using POI.repository.Entities;
using POI.repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace POI.repository.Repositories
{
    public interface IDestinationRepository : IGenericRepository<Destination>
    {
        public IQueryable<Destination> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked);

    }
    public class DestinationRepository : GenericRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(POIContext context) : base(context)
        {

        }

        public IQueryable<Destination> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Destinations
                    .Include(dest => dest.DestinationType)
                    .Include(dest => dest.Province)
                    .Include(dest => dest.DesHashtags)
                    .ThenInclude(destHashtag => destHashtag.Hashtag)
                    .Include(dest => dest.TripDestinations)
                    .ThenInclude(tripDest => tripDest.Trip)
                    .Select(dest => new Destination
                    {
                        DestinationId = dest.DestinationId,
                        DestinationName = dest.DestinationName,
                        Location = dest.Location,
                        Status = dest.Status,
                        DestinationTypeId = dest.DestinationTypeId,
                        ProvinceId = dest.ProvinceId,
                        Province = new Province
                        {
                            ProvinceId = dest.Province.ProvinceId,
                            Name = dest.Province.Name,
                            Status = dest.Province.Status
                        },
                        DestinationType = new DestinationType
                        {
                            DestinationTypeId = dest.DestinationType.DestinationTypeId,
                            Name = dest.DestinationType.Name,
                            Status = dest.DestinationType.Status
                        },
                        DesHashtags = dest.DesHashtags,
                        TripDestinations = dest.TripDestinations
                    })
                    .Where(predicate);
            }
            return _context.Destinations
                    .Include(dest => dest.DestinationType)
                    .Include(dest => dest.Province)
                    .Include(dest => dest.DesHashtags)
                    .ThenInclude(destHashtag => destHashtag.Hashtag)
                    .Include(dest => dest.TripDestinations)
                    .ThenInclude(tripDest => tripDest.Trip)
                    .Select(dest => new Destination
                    {
                        DestinationId = dest.DestinationId,
                        DestinationName = dest.DestinationName,
                        Location = dest.Location,
                        Status = dest.Status,
                        DestinationTypeId = dest.DestinationTypeId,
                        ProvinceId = dest.ProvinceId,
                        Province = new Province
                        {
                            ProvinceId = dest.Province.ProvinceId,
                            Name = dest.Province.Name,
                            Status = dest.Province.Status
                        },
                        DestinationType = new DestinationType
                        {
                            DestinationTypeId = dest.DestinationType.DestinationTypeId,
                            Name = dest.DestinationType.Name,
                            Status = dest.DestinationType.Status
                        },
                        DesHashtags = dest.DesHashtags,
                        TripDestinations = dest.TripDestinations
                    })
                    .Where(predicate);
        }
    }
}
