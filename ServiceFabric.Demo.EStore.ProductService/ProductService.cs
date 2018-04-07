using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using ServiceFabric.Demo.EStore.ProductService.Model;
using Microsoft.ServiceFabric.Data;

namespace ServiceFabric.Demo.EStore.ProductService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    public class ProductService : StatefulService, IProductService
    {
        private const string productsCollection = "products";

        public ProductService(StatefulServiceContext context)
            : base(context)
        { }

        public async Task AddProduct(Product product)
        {
            var products = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(productsCollection);

            using (var tx = StateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(productsCollection);
            var result = new List<Product>();

            using (var tx = StateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        KeyValuePair<Guid, Product> current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }

            return result;
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            var products = await StateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(productsCollection);

            using (var tx = StateManager.CreateTransaction())
            {
                ConditionalValue<Product> product = await products.TryGetValueAsync(tx, productId);

                return product.HasValue ? product.Value : null;
            }
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
