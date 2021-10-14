using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClient;

        public CouponService(IHttpClientFactory httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDto<T>> GetCouponAsync<T>(string couponCode, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.GET,
                Url = ApiHelper.CouponApiBase + "api/coupon/" + couponCode,
                AccessToken = token
            });
        }
    }
}
