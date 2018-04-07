using System;
using System.Fabric;

namespace ServiceFabric.Demo.EStore.Common
{
    public class ServiceUriBuilder
    {
        public ServiceUriBuilder(string serviceInstance)
        {
            ActivationContext = FabricRuntime.GetActivationContext();
            ServiceInstance = serviceInstance;
        }

        public ServiceUriBuilder(ICodePackageActivationContext context, string serviceInstance)
        {
            ActivationContext = context;
            ServiceInstance = serviceInstance;
        }

        public ServiceUriBuilder(ICodePackageActivationContext context, string applicationInstance, string serviceInstance)
        {
            ActivationContext = context;
            ApplicationInstance = applicationInstance;
            ServiceInstance = serviceInstance;
        }

        /// <summary>
        /// The name of the application instance that contains he service.
        /// </summary>
        public string ApplicationInstance { get; set; }

        /// <summary>
        /// The name of the service instance.
        /// </summary>
        public string ServiceInstance { get; set; }

        /// <summary>
        /// The local activation context
        /// </summary>
        public ICodePackageActivationContext ActivationContext { get; set; }

        public Uri ToUri()
        {
            string applicationInstance = ApplicationInstance;

            if (string.IsNullOrEmpty(applicationInstance))
            {
                applicationInstance = ActivationContext.ApplicationName.Replace("fabric:/", string.Empty);
            }

            return new Uri("fabric:/" + applicationInstance + "/" + ServiceInstance);
        }
    }
}
