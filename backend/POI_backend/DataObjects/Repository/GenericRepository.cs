using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DataObjects.IRepository;
using BusinessObjects;

namespace DataObjects.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected readonly POIContext _context;

        public GenericRepository(POIContext context)
        {
            // Inject POIcontext here
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetByID(int ID)
        {
            return _context.Set<T>().Find(ID);
        }

        public T GetByID(Guid ID)
        {
            return _context.Set<T>().Find(ID);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
    }
}
