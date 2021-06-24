using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;

namespace POI.repository.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
    }
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(POIContext context) : base(context)
        {

        }
    }
}
