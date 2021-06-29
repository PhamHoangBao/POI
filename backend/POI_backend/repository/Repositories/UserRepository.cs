using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POI.repository.Entities;
using Microsoft.EntityFrameworkCore;


namespace POI.repository.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserWithRoleAsync(Expression<Func<User, bool>> predicate, bool istracked);
        User GetUserWithRole(Expression<Func<User, bool>> predicate, bool istracked);

    }
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(POIContext context) : base(context)
        {

        }

        public User GetUserWithRole(Expression<Func<User, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Users
                    .Include(user => user.Role)
                    .FirstOrDefault(predicate);
            }
            return this._context.Users
                .Include(user => user.Role)
                .AsNoTracking()
                .FirstOrDefault(predicate);
        }

        public async Task<User> GetUserWithRoleAsync(Expression<Func<User, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return await _context.Users
                    .Include(user => user.Role)
                    .FirstOrDefaultAsync(predicate);
            }
            return await _context.Users
                .Include(user => user.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }
    }
}
