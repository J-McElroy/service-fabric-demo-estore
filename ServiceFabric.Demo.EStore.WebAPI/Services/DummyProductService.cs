using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceFabric.Demo.EStore.ProductService.Model;

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

        public Task<Guid> AddProduct(Product product)
        {
            product.Id = Guid.NewGuid();
            products.Add(product);
            return Task.FromResult(product.Id);
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
