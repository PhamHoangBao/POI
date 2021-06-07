using System;
using System.Collections.Generic;
using System.Text;
using POI.repository.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace POI.repository.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserWithRoleAsync(Expression<Func<User, bool>> predicate, bool istracked);
        User GetUserWithRole(Expression<Func<User, bool>> predicate, bool istracked);
    }
}
