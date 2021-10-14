using Mango.Services.Shopping.Models.Dtos;
using Newtonsoft.Json;

namespace Mango.Services.Shopping.Services
{
    public class CouponRespository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRespository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await _httpClient.GetAsync($"/api/coupon/{couponName}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto<CouponDto>>(apiContent);
            if (resp.Success)
                return resp.Result;
            else
                return new CouponDto();
        }
    }
}
