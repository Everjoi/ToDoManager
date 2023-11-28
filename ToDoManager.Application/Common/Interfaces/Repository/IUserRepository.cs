using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Domain.Entities;

namespace ToDoManager.Application.Common.Interfaces.Repository
{
    public interface IUserRepository:IGenericRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}
