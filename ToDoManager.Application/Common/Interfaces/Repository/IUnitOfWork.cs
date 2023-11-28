using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Domain.Common.Interfaces;

namespace ToDoManager.Application.Common.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class, IEntity;
        Task<int> Save(CancellationToken cancellationToken);
        Task<int> SaveAndRemoveCache(CancellationToken cancellationToken,params string[] cacheKeys);
        Task Rollback();
    }
}
