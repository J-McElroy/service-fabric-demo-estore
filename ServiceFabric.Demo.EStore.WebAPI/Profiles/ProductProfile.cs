using AutoMapper;
using ServiceFabric.Demo.EStore.ProductService.Model;
using ServiceFabric.Demo.EStore.WebAPI.Model;

namespace ServiceFabric.Demo.EStore.WebAPI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ApiProduct, Product>().ReverseMap();
        }
    }
}
