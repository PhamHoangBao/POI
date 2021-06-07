using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using POI.repository.IRepositories;
using POI.repository.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace POI.repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly POIContext _context;

        public GenericRepository(POIContext context)
        {
            // Inject POIContext here
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return _context.Set<T>().FirstOrDefault(predicate);
            }
            return _context.Set<T>().AsNoTracking().FirstOrDefault(predicate);

        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool istracked)
        {
            if (istracked)
            {
                return await _context.Set<T>().FirstOrDefaultAsync(predicate);
            }
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetByIDAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }


        void IGenericRepository<T>.Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        void IGenericRepository<T>.AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        IQueryable<T> IGenericRepository<T>.Find(Expression<Func<T, bool>> expression, bool istracked)
        {
            if (istracked)
            {
                return _context.Set<T>().Where(expression);
            }
            return _context.Set<T>().AsNoTracking().Where(expression);

        }

        IEnumerable<T> IGenericRepository<T>.GetAll()
        {
            return _context.Set<T>().ToList();
        }

        T IGenericRepository<T>.GetByID(Guid ID)
        {

            return _context.Set<T>().Find(ID);
        }

        void IGenericRepository<T>.Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        void IGenericRepository<T>.RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
