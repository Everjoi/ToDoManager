using Microsoft.EntityFrameworkCore;
using ToDoManager.Domain.Entities;

namespace ToDoManager.Persistance.Data.Context
{
    public class ManagerContext : DbContext
    {
        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) { }


        public DbSet<User> Users => Set<User>();
        public DbSet<ToDoManager.Domain.Entities.Task> Tasks => Set<ToDoManager.Domain.Entities.Task>();
            
    }
}
