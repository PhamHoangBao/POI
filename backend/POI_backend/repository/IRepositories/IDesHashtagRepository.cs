using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using POI.repository.IRepositories;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace POI.repository.IRepositories
{
    public interface IDesHashtagRepository : IGenericRepository<DesHashtag>
    {
        public IQueryable<DesHashtag> GetDestHashtag(Expression<Func<DesHashtag, bool>> predicate, bool istracked);
    }
}
