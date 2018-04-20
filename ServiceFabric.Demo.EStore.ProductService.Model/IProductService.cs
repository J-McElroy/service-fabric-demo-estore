using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace ServiceFabric.Demo.EStore.ProductService.Model
{
    public interface IProductService : IService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProduct(Guid productId);

        Task<Guid> AddProduct(Product product);
    }
}
