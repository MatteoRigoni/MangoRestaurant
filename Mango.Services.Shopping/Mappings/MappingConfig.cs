using AutoMapper;
using Mango.Services.Shopping.Models;
using Mango.Services.Shopping.Models.Dtos;

namespace Mango.Services.Shopping.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                config.CreateMap<CartDetailDto, CartDetail>().ReverseMap();
                config.CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
                config.CreateMap<CartDto, Cart>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
