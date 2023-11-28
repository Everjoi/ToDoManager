using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;
using ToDoManager.Persistance.ModuleContainer;

namespace ToDoManager.Persistance.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddPersistenceLayer(this IServiceCollection services,IConfiguration configuration,IHostBuilder host)
        {
            services.AddDbContext(configuration);
            host.AddRepositories();
        }


        public static void AddDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ManagerContext>(options =>
               options.UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(ManagerContext).Assembly.FullName)));
        }


        private static void AddRepositories(this IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory()).
                ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AppModule());
                });
        }
    }
}
