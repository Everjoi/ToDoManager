using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Domain.Entities;
using Task = ToDoManager.Domain.Entities.Task;

namespace ToDoManager.Application.Common.Interfaces.Repository
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        Task<List<Task>> GetTasksByUserId(Guid userId);
    }
}
