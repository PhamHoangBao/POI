using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace POI.repository.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        // Here is 8 basic methods for an entity.
        T GetByID(Guid ID);
        Task<T> GetByIDAsync(Guid id);
        IEnumerable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression, bool istracked);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool istracked);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, bool istracked);
        void Update(T entity);
        Task AddAsync(T entity);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
