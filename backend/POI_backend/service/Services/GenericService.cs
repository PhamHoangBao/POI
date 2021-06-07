using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using POI.service.IServices;
using POI.repository.IRepositories;


namespace POI.service.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        public readonly IGenericRepository<T> _genericRepository;

        public GenericService()
        {

        }
        public GenericService(IGenericRepository<T> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public void Add(T entity)
        {
            _genericRepository.Add(entity);
        }

        public Task AddAsync(T entity)
        {
           return _genericRepository.AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _genericRepository.AddRange(entities);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression, bool istracked)
        {
            return _genericRepository.Find(expression, istracked);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, bool istracked)
        {
            return _genericRepository.FirstOrDefault(predicate, istracked);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool istracked)
        {
            return _genericRepository.FirstOrDefaultAsync(predicate, istracked);
        }

        public IEnumerable<T> GetAll()
        {
            return _genericRepository.GetAll();
        }
        public T GetByID(Guid ID)
        {
            return _genericRepository.GetByID(ID);
        }

        public Task<T> GetByIDAsync(Guid id)
        {
            return _genericRepository.GetByIDAsync(id);
        }

        public void Remove(T entity)
        {
            _genericRepository.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _genericRepository.RemoveRange(entities);
        }

        public void Savechanges()
        {
            _genericRepository.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _genericRepository.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _genericRepository.Update(entity);
        }
    }
}
