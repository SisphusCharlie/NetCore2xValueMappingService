using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ValueMappingCoreAPI.Repository;

namespace ValueMappingCoreAPI.IOC
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<SystemRepository>().As<ISystemRepository>();
            builder.RegisterType<ValueMapsRepository>().As<IValueMapsRepository>();



            //builder.RegisterGeneric(typeof(SystemRepository)).As(typeof(ISystemRepository)).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(ValueMapsRepository)).As(typeof(IValueMapsRepository)).InstancePerLifetimeScope();

            //builder.RegisterType<Task<SystemRepository>>().As<Task<ISystemRepository>>();
            //builder.RegisterType<Task<ValueMapsRepository>>().As<Task<IValueMapsRepository>>();
        }
    }
}
