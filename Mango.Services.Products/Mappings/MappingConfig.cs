using AutoMapper;
using Mango.Services.Products.Models;
using Mango.Services.Products.Models.Dtos;

namespace Mango.Services.Products.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
