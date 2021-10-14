using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public interface ICartService
    {
        Task<ResponseDto<T>> GetCartByUserIdAsync<T>(string userId, string token);
        Task<ResponseDto<T>> AddToCartAsync<T>(CartDto cartDto, string token);
        Task<ResponseDto<T>> UpdateCartAsync<T>(CartDto cartDto, string token);
        Task<ResponseDto<T>> RemoveFromCartAsync<T>(int cartId, string token);
        Task<ResponseDto<T>> ApplyCouponAsync<T>(CartDto cartDto, string token);
        Task<ResponseDto<T>> RemoveCouponAsync<T>(string userId, string token);
        Task<ResponseDto<T>> CheckoutAsync<T>(CartHeaderDto cartHeader, string token);
    }
}
