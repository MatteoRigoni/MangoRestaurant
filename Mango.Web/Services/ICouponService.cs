using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public interface ICouponService
    {
        Task<ResponseDto<T>> GetCouponAsync<T>(string couponCode, string token);
    }
}
