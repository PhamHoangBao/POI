using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace POI.service.IServices
{
    public interface IGenericService<T> where T : class
    {
        T GetByID(Guid ID);
        Task<T> GetByIDAsync(Guid id);
        IEnumerable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression, bool istracked);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool istracked);
        T FirstOrDefault(Expression<Func<T, bool>> predicate, bool istracked);
        Task AddAsync(T entity);
        void Update(T entity);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Savechanges();
        Task SaveChangesAsync();
    }
}
