using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POI.repository.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;



namespace POI.repository.Repositories
{
    public interface IDesHashtagRepository : IGenericRepository<DesHashtag>
    {
        public IQueryable<DesHashtag> GetDestHashtag(Expression<Func<DesHashtag, bool>> predicate, bool istracked);
    }
    public class DesHashtagRepository : GenericRepository<DesHashtag>, IDesHashtagRepository
    {
        public DesHashtagRepository(POIContext context) : base(context)
        {

        }

        public IQueryable<DesHashtag> GetDestHashtag(Expression<Func<DesHashtag, bool>> predicate, bool istracked)
        {
            return _context.DesHashtags
                .Include(destHashtag => destHashtag.Hashtag)
                .Include(destHashtag => destHashtag.Destination).ThenInclude(dest => dest.Province)
                .Include(destHashtag => destHashtag.Destination).ThenInclude(dest => dest.DestinationType)
                .Select(destHashtag => new DesHashtag
                {
                    DesHashtagId = destHashtag.DesHashtagId,
                    DestinationId = destHashtag.DestinationId,
                    HashtagId = destHashtag.HashtagId,
                    Destination = new Destination
                    {
                        DestinationId = destHashtag.Destination.DestinationId,
                        DestinationName = destHashtag.Destination.DestinationName,
                        Status = destHashtag.Destination.Status,
                        DestinationTypeId = destHashtag.Destination.DestinationTypeId,
                        ProvinceId = destHashtag.Destination.ProvinceId,
                        Province = destHashtag.Destination.Province,
                        DestinationType = destHashtag.Destination.DestinationType
                    },
                    Hashtag = new Hashtag
                    {
                        HashtagId = destHashtag.Hashtag.HashtagId,
                        Name = destHashtag.Hashtag.Name,
                        ShortName = destHashtag.Hashtag.ShortName,
                        Status = destHashtag.Status
                    },
                    Status = destHashtag.Status
                })
                .Where(predicate);
        }
    }
}
