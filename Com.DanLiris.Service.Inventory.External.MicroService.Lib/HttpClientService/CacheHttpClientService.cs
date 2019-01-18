using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib.HttpClientService
{
    public class CacheHttpClientService : ICacheHttpClientService
    {
        static HttpClient _client = new HttpClient();

        //public HttpClientService(string token)
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //}

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string token, HttpContent content)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _client.PostAsync(url, content);
        }
    }
}
