using ServiceFabric.Demo.EStore.ProductService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.WebAPI.Services
{
    public class DummyProductService : IProductService
    {
        private readonly List<Product> products = new List<Product>()
            {
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Dummy product 1",
                    Price = 50
                },
                new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = "Dummy product 2",
                    Price = 150
                }
            };

        public Task AddProduct(Product product)
        {
            products.Add(product);
            return Task.FromResult(0);
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return Task.FromResult((IEnumerable<Product>)products);
        }

        public Task<Product> GetProduct(Guid productId)
        {
            return Task.FromResult(products.FirstOrDefault(el => el.Id == productId));
        }
    }
}
