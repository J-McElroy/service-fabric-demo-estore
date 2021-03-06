﻿using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ServiceFabric.Demo.EStore.ProductService.Settings;

namespace ServiceFabric.Demo.EStore.ProductService
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

                builder.RegisterInstance(FabricRuntime.GetActivationContext()).As<CodePackageActivationContext>()
                    .SingleInstance();

                builder.RegisterType<ProductServiceSettings>().As<IProductServiceSettings>().SingleInstance();

                builder.RegisterStatefulService<ProductService>("ServiceFabric.Demo.EStore.ProductServiceType");

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ProductService).Name);
                using (builder.Build())
                {
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
