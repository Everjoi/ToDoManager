﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Domain.Common.Interfaces;

namespace ToDoManager.Application.Common.Interfaces.Repository
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
