using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ResponseDto<T>> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient();
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case ApiHelper.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiHelper.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiHelper.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto<T>>(apiContent);

                return apiResponseDto;
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto<T>()
                {
                    DisplayMessage = "Error",
                    Errors = new List<string> { Convert.ToString(ex.Message) },
                    Success = false
                };

                return dto;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
