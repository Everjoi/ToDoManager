using Autofac;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Application.Common;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Infrastructure.Services;
using ToDoManager.Persistance.Data.Context;
using ToDoManager.Persistance.Repository;

namespace ToDoManager.Persistance.ModuleContainer
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                 .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<MemoryCache>().As<IMemoryCache>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ManagerContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<TaskRepository>().As<ITaskRepository>().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
