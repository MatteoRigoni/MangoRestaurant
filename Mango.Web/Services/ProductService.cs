using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _httpClient;

        public ProductService(IHttpClientFactory httpClient) :base(httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDto<T>> CreateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest() { 
                ApiType = ApiHelper.ApiType.POST ,
                Data = productDto,
                Url = ApiHelper.MangoApiBase + "api/products",
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> DeleteProductAsync<T>(int productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.DELETE,
                Url = ApiHelper.MangoApiBase + "api/products/" + productId,
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> GetAllProductByIdAsync<T>(int productId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.GET,
                Url = ApiHelper.MangoApiBase + "api/products/" + productId,
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> GetAllProductsAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.GET,
                Url = ApiHelper.MangoApiBase + "api/products",
                AccessToken = token
            });
        }

        public async Task<ResponseDto<T>> UpdateProductAsync<T>(ProductDto productDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiHelper.ApiType.PUT,
                Data = productDto,
                Url = ApiHelper.MangoApiBase + "api/products",
                AccessToken = token
            });
        }
    }
}
