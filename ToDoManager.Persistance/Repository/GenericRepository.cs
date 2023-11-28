using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Domain.Common.Interfaces;
using ToDoManager.Domain.Exceptions;
using ToDoManager.Persistance.Data.Context;

namespace ToDoManager.Persistance.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected readonly ManagerContext _dbContext;

        public GenericRepository(ManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            if(await _dbContext.Set<T>().AnyAsync(x => x.Id == entity.Id))
                throw new Exception();

            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            if(!_dbContext.Set<T>().AnyAsync(x => x.Id == entity.Id).Result)
                throw new EntityException($"Entity: {entity.Id.ToString()} not found");

            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            if(!_dbContext.Set<T>().AnyAsync(x => x.Id == entity.Id).Result)
                throw new EntityException($"Entity: {entity.Id.ToString()} not found");

            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var result = await _dbContext.Set<T>().FindAsync(id);

            if(result == null)
                throw new EntityException($"Entity: {id.ToString()} not found");

            return result;
        }


    }
}
