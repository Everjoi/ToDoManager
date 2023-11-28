using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Domain.Entities;
using ToDoManager.Persistance.Data.Context;

namespace ToDoManager.Persistance.Repository
{
    public class TaskRepository:GenericRepository<ToDoManager.Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(ManagerContext dbContext) : base(dbContext) { }

        public async Task<List<Domain.Entities.Task>> GetTasksByUserId(Guid userId)
        {
            var result = await _dbContext.Tasks.Where(x => x.UserId == userId).ToListAsync();
            return result;
        }
    }
}
