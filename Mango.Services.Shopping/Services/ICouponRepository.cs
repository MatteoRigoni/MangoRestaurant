using Mango.Services.Shopping.Models.Dtos;

namespace Mango.Services.Shopping.Services
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
