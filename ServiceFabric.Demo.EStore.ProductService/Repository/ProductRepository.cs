using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceFabric.Demo.EStore.ProductService.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Threading;

namespace ServiceFabric.Demo.EStore.ProductService.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private const string productsCollection = "products";
        private readonly IReliableStateManager stateManager;

        public ProductRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task AddProduct(Product product)
        {
            var products = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(productsCollection);

            using (var tx = stateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);

                await tx.CommitAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(productsCollection);
            var result = new List<Product>();

            using (var tx = stateManager.CreateTransaction())
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
            var products = await stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(productsCollection);

            using (var tx = stateManager.CreateTransaction())
            {
                ConditionalValue<Product> product = await products.TryGetValueAsync(tx, productId);

                return product.HasValue ? product.Value : null;
            }
        }
    }
}
