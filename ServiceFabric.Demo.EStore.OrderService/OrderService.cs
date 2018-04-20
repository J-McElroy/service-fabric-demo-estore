using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using ServiceFabric.Demo.EStore.CartActor.Interfaces;
using ServiceFabric.Demo.EStore.Common;
using ServiceFabric.Demo.EStore.OrderService.Model;
using ServiceFabric.Demo.EStore.OrderService.Model.Exceptions;
using ServiceFabric.Demo.EStore.OrderService.Model.Model;
using ServiceFabric.Demo.EStore.ProductService.Model;

namespace ServiceFabric.Demo.EStore.OrderService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public class OrderService : StatelessService, IOrderService
    {
        private readonly IActorProxyFactory actorProxyFactory;
        private readonly IProductService productService;

        public OrderService(StatelessServiceContext context, IActorProxyFactory actorProxyFactory, IProductService productService)
            : base(context)
        {
            this.actorProxyFactory = actorProxyFactory;
            this.productService = productService;
        }

        public async Task AddToCart(string userId, Guid productId, int quantity)
        {
            var product = await productService.GetProduct(productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            var cartActor = GetCartActor(userId);

            await cartActor.Add(productId, quantity);
        }

        public async Task<CartModel> GetCart(string userId)
        {
            var cartActor = GetCartActor(userId);

            var cart = await cartActor.GetItems();

            var model = new CartModel() { Items = new List<CartItem>() };

            foreach (var productId in cart.Keys)
            {
                var product = await productService.GetProduct(productId);

                if (product != null)
                {
                    model.Items.Add(new CartItem()
                    {
                        ProductId = productId,
                        ProductName = product.Name,
                        ProductPrice = product.Price,
                        Quantity = cart[productId]
                    });
                }
            }

            return model;
        }

        public async Task ClearCart(string userId)
        {
            var cartActor = GetCartActor(userId);

            await cartActor.Clear();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(this.CreateServiceRemotingListener)
            };
        }

        private ICartActor GetCartActor(string bookingId)
        {
            return actorProxyFactory.CreateActorProxy<ICartActor>(
                new ServiceUriBuilder(ServiceNames.CartActorName).ToUri(),
                new ActorId(bookingId));
        }
    }
}
