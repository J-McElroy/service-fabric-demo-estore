using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceFabric.Demo.EStore.ProductService.Model;
using ServiceFabric.Demo.EStore.WebAPI.Model;
using AutoMapper;

namespace ServiceFabric.Demo.EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> Get()
        {
            IEnumerable<Product> allProducts = await productService.GetAllProducts();

            return mapper.Map<IEnumerable<ApiProduct>>(allProducts);
        }

        [HttpPost]
        public Task<Guid> Post([FromBody] ApiProduct product)
        {
            var newProduct = mapper.Map<Product>(product);

            return productService.AddProduct(newProduct);
        }
    }
}
