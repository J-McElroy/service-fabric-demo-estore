using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Autofac.Integration.ServiceFabric;
using Microsoft.ServiceFabric.Actors.Client;
using ServiceFabric.Demo.EStore.ProductService.Model;
using ServiceFabric.Demo.EStore.Common;
using Microsoft.ServiceFabric.Services.Client;

namespace ServiceFabric.Demo.EStore.OrderService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                var builder = new ContainerBuilder();

                builder.RegisterServiceFabricSupport();
                builder.Register(c => new ServiceProxyFactory()).As<IServiceProxyFactory>();
                builder.Register(c => new ActorProxyFactory()).As<IActorProxyFactory>();

                builder.Register<IProductService>(c =>
                {
                    var factory = c.Resolve<IServiceProxyFactory>();
                    var uriBuilder = new ServiceUriBuilder(ServiceNames.ProductServiceName);
                    return factory.CreateServiceProxy<IProductService>(uriBuilder.ToUri(), new ServicePartitionKey(0));
                });

                builder.RegisterStatelessService<OrderService>("ServiceFabric.Demo.EStore.OrderServiceType");

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(OrderService).Name);
                using (builder.Build())
                {
                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
