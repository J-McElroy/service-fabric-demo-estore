﻿using ServiceFabric.Demo.EStore.ProductService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.ProductService.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProduct(Guid productId);

        Task AddProduct(Product product);
    }
}
