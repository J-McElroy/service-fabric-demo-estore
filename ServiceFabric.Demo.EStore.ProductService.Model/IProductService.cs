﻿using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.ProductService.Model
{
    public interface IProductService : IService
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProduct(Guid productId);

        Task AddProduct(Product product);

    }
}