using Mango.Web.Models;
using Mango.Web.Models.Dtos;

namespace Mango.Web.Services
{
    public interface IBaseService: IDisposable
    {
        Task<ResponseDto<T>> SendAsync<T>(ApiRequest apiRequest);
    }
}
