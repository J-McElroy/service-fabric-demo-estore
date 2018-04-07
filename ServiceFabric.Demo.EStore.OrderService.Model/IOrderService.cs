using Microsoft.ServiceFabric.Services.Remoting;
using ServiceFabric.Demo.EStore.OrderService.Model.Model;
using System;
using System.Threading.Tasks;

namespace ServiceFabric.Demo.EStore.OrderService.Model
{
    public interface IOrderService : IService
    {
        Task AddToCart(string userId, Guid productId, int quantity);

        Task<CartModel> GetCart(string userId);

        Task ClearCart(string userId);
    }
}
