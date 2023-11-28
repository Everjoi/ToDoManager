using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoManager.Persistance.Data.Context;

namespace ToDoManager.Persistance.Extensions
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialization(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ManagerContext>()?.Database.Migrate();
            }
        }
    }
}
