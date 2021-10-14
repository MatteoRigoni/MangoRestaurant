using Mango.Services.Coupon.Models.Dtos;

namespace Mango.Services.Coupon.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
