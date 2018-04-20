using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceFabric.Demo.EStore.ProductService.Model;

namespace ServiceFabric.Demo.EStore.ProductService.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProduct(Guid productId);

        Task AddProduct(Product product);
    }
}
