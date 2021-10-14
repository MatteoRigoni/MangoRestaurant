using AutoMapper;
using Mango.Services.Coupon.Models;
using Mango.Services.Coupon.Models.Dtos;

namespace Mango.Services.Coupon.Mappings
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon.Models.Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
