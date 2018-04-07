using AutoMapper;
using ServiceFabric.Demo.EStore.OrderService.Model.Model;
using ServiceFabric.Demo.EStore.WebAPI.Model;

namespace ServiceFabric.Demo.EStore.WebAPI.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartModel, ApiCart>();
            CreateMap<CartItem, ApiCartItem>();
        }
    }
}
