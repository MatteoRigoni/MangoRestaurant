using AutoMapper;
using Mango.Services.Orders.Models;

namespace Mango.Services.Orders.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                //config.CreateMap<CouponDto, Coupon.Models.Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
