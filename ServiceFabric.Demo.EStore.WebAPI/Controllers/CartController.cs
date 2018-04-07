using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServiceFabric.Demo.EStore.OrderService.Model;
using ServiceFabric.Demo.EStore.OrderService.Model.Exceptions;
using ServiceFabric.Demo.EStore.WebAPI.Model;
using System;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public CartController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<ApiCart> Get(string userId)
        {
            var cartModel = await orderService.GetCart(userId);

            return mapper.Map<ApiCart>(cartModel);
        }

        [HttpPost("{userId}")]
        public async Task Add(string userId, [FromBody] ApiCartAddRequest request)
        {
            try
            {
                await orderService.AddToCart(userId, request.ProductId, request.Quantity);
            }
            catch(AggregateException ex)
            {
                var exception = ex.GetBaseException();

                if(exception is ProductNotFoundException)
                {
                    // TODO: return appropriate status code
                }
            }
        }

        [HttpDelete("{userId}")]
        public Task Delete(string userId)
        {
            return orderService.ClearCart(userId);
        }
    }
}
