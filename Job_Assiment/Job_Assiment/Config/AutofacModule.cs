using Assiment.core.Interfaces;
using Assiment.EF;
using Assiment.EF.Repositories;
using Assiment.EF.Services;
using Autofac;
using ECommerce.Data.UnitOfWork;

namespace Assiment.Config
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ApplicationDbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).InstancePerLifetimeScope();
           builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
           builder.RegisterAssemblyTypes(typeof(ProductService).Assembly).InstancePerLifetimeScope();
        }
    }
}
