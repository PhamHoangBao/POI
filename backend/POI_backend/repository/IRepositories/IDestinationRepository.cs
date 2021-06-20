using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;


namespace POI.repository.IRepositories
{
    public interface IDestinationRepository : IGenericRepository<Destination>
    {
        public IQueryable<Destination> GetDestination(Expression<Func<Destination, bool>> predicate, bool istracked);

    }
}
