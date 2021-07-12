using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using POI.repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace POI.repository.Repositories
{
    public interface IVisitRepository : IGenericRepository<Visit>
    {
        public IQueryable<Visit> GetVisits(Expression<Func<Visit, bool>> predicate, bool istracked);
        public Tuple<double,double> CalculateRating(Guid poiId);
    }
    public class VisitRepository : GenericRepository<Visit> , IVisitRepository
    {
        public VisitRepository(POIContext context) : base(context)
        {

        }

        public Tuple<double, double> CalculateRating(Guid poiId)
        {
            var poiRatings = _context.Visits
                .Select(p => new { Rating = p.Rating, PoiId = p.PoiId })
                .Where(m => m.PoiId.Equals(poiId) && m.Rating != -1).ToList();
            var numberOfRating = poiRatings.Count();
            double totalRating = 0;
            for(int i = 0; i < numberOfRating; i++)
            {
                var m = poiRatings[i];
                totalRating += m.Rating;
            }
            Console.WriteLine("poiID : " + poiId.ToString());
            Console.WriteLine("Total : " + totalRating + " num rating : " + numberOfRating);
            return new Tuple<double, double>(totalRating, numberOfRating);
        }

        public IQueryable<Visit> GetVisits(Expression<Func<Visit, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Visits
                    .Include(m => m.Poi)
                    .Select(m => new Visit
                    {
                        VisitId = m.VisitId,
                        PoiId = m.PoiId,
                        Rating = m.Rating,
                        VisitDate = m.VisitDate,
                        Poi = m.Poi
                    })
                    .Where(predicate);
            } else
            {
                return _context.Visits
                    .Include(m => m.Poi)
                    .Select(m => new Visit
                    {
                        VisitId = m.VisitId,
                        PoiId = m.PoiId,
                        Rating = m.Rating,
                        VisitDate = m.VisitDate,
                        Poi = m.Poi
                    })
                    .Where(predicate)
                    .AsNoTracking();
            }
        }
    }
}
