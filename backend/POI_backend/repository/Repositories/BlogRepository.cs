using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POI.repository.Enums;


namespace POI.repository.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        public IQueryable<Blog> GetBlogs(Expression<Func<Blog, bool>> predicate, bool istracked);

    }
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(POIContext context) : base(context)
        {

        }

        public IQueryable<Blog> GetBlogs(Expression<Func<Blog, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Blogs
                    .Include(m => m.User)
                    .Include(m => m.Trip)
                    .Select(m => new Blog
                    {
                        BlogId = m.BlogId,
                        TripId = m.TripId,
                        Trip = m.Trip,
                        Title = m.Title,
                        Content = m.Content,
                        UserId = m.UserId,
                        User = m.User,
                        PosVotes = m.PosVotes,
                        NegVotes = m.NegVotes,
                        Status = m.Status
                    })
                    .Where(predicate);
            }
            else
            {
                return _context.Blogs
                    .Include(m => m.User)
                    .Include(m => m.Trip)
                    .Select(m => new Blog
                    {
                        BlogId = m.BlogId,
                        TripId = m.TripId,
                        Trip = m.Trip,
                        UserId = m.UserId,
                        Title = m.Title,
                        Content = m.Content,
                        User = m.User,
                        PosVotes = m.PosVotes,
                        NegVotes = m.NegVotes,
                        Status = m.Status
                    })
                    .Where(predicate)
                    .AsNoTracking();
            }
        }

    }
}
