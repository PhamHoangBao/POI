using POI.repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POI.repository.Repositories
{
    public interface IPoiRepository : IGenericRepository<Poi>
    {
        public IQueryable<Poi> GetPoi(Expression<Func<Poi, bool>> predicate, bool istracked);
    }
    public class PoiRepository : GenericRepository<Poi>, IPoiRepository
    {
        public PoiRepository(POIContext context) : base(context)
        {

        }

        public IQueryable<Poi> GetPoi(Expression<Func<Poi, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Pois
                    .Include(poi => poi.Destination)
                    .Include(poi => poi.PoiType)
                    .Include(poi => poi.User)
                    .Select(poi => new Poi
                    {
                        PoiId = poi.PoiId,
                        Name = poi.Name,
                        Location = poi.Location,
                        Description = poi.Description,
                        Rating = poi.Rating,
                        DestinationId = poi.DestinationId,
                        Status = poi.Status,
                        UserId = poi.UserId,
                        ImageUrl = poi.ImageUrl,
                        User = poi.User,
                        PoiType = poi.PoiType,
                        Destination = poi.Destination,
                    })
                    .Where(predicate);
            }
            else
            {
                return _context.Pois
                   .Include(poi => poi.Destination)
                   .Include(poi => poi.PoiType)
                   .Include(poi => poi.User)
                   .Select(poi => new Poi
                   {
                       PoiId = poi.PoiId,
                       Name = poi.Name,
                       Location = poi.Location,
                       Description = poi.Description,
                       DestinationId = poi.DestinationId,
                       Rating = poi.Rating,
                       UserId = poi.UserId,
                       Status = poi.Status,
                       ImageUrl = poi.ImageUrl,
                       User = poi.User,
                       PoiType = poi.PoiType,
                       Destination = poi.Destination,
                   })
                   .Where(predicate)
                   .AsNoTracking();
            }
        }

    }
}
