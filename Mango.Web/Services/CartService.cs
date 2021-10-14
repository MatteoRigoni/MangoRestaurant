using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory _httpClient;

        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDto<T>> AddToCartAsync<T>(CartDto cartDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.POST,
                Data = cartDto,
                Url = ApiHelper.ShoppingApiBase + "api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> ApplyCouponAsync<T>(CartDto cartDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.POST,
                Data = cartDto,
                Url = ApiHelper.ShoppingApiBase + "api/cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async  Task<ResponseDto<T>> CheckoutAsync<T>(CartHeaderDto cartHeader, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.POST,
                Data = cartHeader,
                Url = ApiHelper.ShoppingApiBase + "api/cart/checkout",
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> GetCartByUserIdAsync<T>(string userId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.GET,
                Url = ApiHelper.ShoppingApiBase + "api/cart/GetCart/" + userId,
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> RemoveCouponAsync<T>(string userId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.POST,
                Data = userId,
                Url = ApiHelper.ShoppingApiBase + "api/cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> RemoveFromCartAsync<T>(int cartId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.POST,
                Url = ApiHelper.ShoppingApiBase + "api/cart/RemoveCart/" + cartId,
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> UpdateCartAsync<T>(CartDto cartDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.POST,
                Data = cartDto,
                Url = ApiHelper.ShoppingApiBase + "api/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}
