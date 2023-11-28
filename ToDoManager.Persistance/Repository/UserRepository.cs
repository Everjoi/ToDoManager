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
    public class UserRepository:GenericRepository<User>, IUserRepository
    {

        public UserRepository(ManagerContext dbContext) : base(dbContext) { }


        public async Task<User> GetUserByEmail(string email)
        {
            var result = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            return result;
        }

    }
}
