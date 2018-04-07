using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceFabric.Demo.EStore.Common
{
    public static class AutofacExtentions
    {
        public static ContainerBuilder UseAutoMapper(this ContainerBuilder containerBuilder, Assembly assembly)
        {
            containerBuilder.Register(BuildMapper)
                .As<IMapper>()
                .SingleInstance();

            containerBuilder
                .RegisterAssemblyInheritedTypes<Profile>(assembly)
                .As<Profile>();

            return containerBuilder;
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> RegisterAssemblyInheritedTypes<TBaseClass>(
            this ContainerBuilder containerBuilder,
            Assembly assembly = null)
        {
            assembly = assembly ?? Assembly.GetCallingAssembly();

            return containerBuilder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(TBaseClass).IsAssignableFrom(t));
        }

        private static IMapper BuildMapper(IComponentContext context)
        {
            var profiles = context.Resolve<IEnumerable<Profile>>();
            var configuration = new MapperConfiguration(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            return configuration.CreateMapper();
        }
    }
}
