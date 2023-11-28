using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Domain.Common.Interfaces;
using ToDoManager.Persistance.Data.Context;


namespace ToDoManager.Persistance.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ManagerContext _dbContext;
        private readonly IMemoryCache _cache;
        private Hashtable _repositories;
        private bool disposed;

        public UnitOfWork(ManagerContext dbContext,IMemoryCache cache)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _cache = cache;

        }

        public IGenericRepository<T> Repository<T>() where T : class, IEntity
        {
            if(_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(T).Name;

            if(!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)),_dbContext);

                _repositories.Add(type,repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public Task Rollback()
        {
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public async Task<int> Save(CancellationToken cancellationToken)
        {
            await SaveAndRemoveCache(cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveAndRemoveCache(CancellationToken cancellationToken,params string[] cacheKeys)
        {
            var changesCount = await _dbContext.SaveChangesAsync(cancellationToken);

            foreach(var key in cacheKeys)
            {
                _cache.Remove(key);
            }

            return changesCount;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed)
                return;

            if(disposing)
            {
                _dbContext.Dispose();
            }
            disposed = true;
        }

    }
}
