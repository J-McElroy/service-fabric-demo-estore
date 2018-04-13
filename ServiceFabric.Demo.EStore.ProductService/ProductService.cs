using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using ServiceFabric.Demo.EStore.ProductService.Model;
using ServiceFabric.Demo.EStore.ProductService.Settings;
using ServiceFabric.Demo.EStore.ProductService.Repository;

namespace ServiceFabric.Demo.EStore.ProductService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public class ProductService : StatefulService, IProductService
    {
        private readonly IProductServiceSettings settings;
        private readonly IProductRepository repository;

        public ProductService(StatefulServiceContext context, IProductServiceSettings settings)
            : base(context)
        {            
            this.settings = settings;
            // TODO: think of a way to inject IReliableStateManager
            repository = new ProductRepository(StateManager);
        }

        public async Task<Guid> AddProduct(Product product)
        {
            product.Id = Guid.NewGuid();

            if (string.IsNullOrEmpty(product.Description))
            {
                product.Description = settings.DefaultDescription;
            }

            await repository.AddProduct(product);

            return product.Id;
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return repository.GetAllProducts();
        }

        public Task<Product> GetProduct(Guid productId)
        {
            return repository.GetProduct(productId);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
         {
                new ServiceReplicaListener(context => this.CreateServiceRemotingListener(context))
         };
        }
    }
}
