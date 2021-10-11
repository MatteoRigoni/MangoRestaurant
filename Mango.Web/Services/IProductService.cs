using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public interface IProductService
    {
        Task<ResponseDto<T>> GetAllProductsAsync<T>(string token);
        Task<ResponseDto<T>> GetAllProductByIdAsync<T>(int productId, string token);
        Task<ResponseDto<T>> CreateProductAsync<T>(ProductDto product, string token);
        Task<ResponseDto<T>> UpdateProductAsync<T>(ProductDto product, string token);
        Task<ResponseDto<T>> DeleteProductAsync<T>(int productId, string token);
    }
}
