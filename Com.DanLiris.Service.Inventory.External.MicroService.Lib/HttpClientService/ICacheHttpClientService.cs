using System.Net.Http;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib.HttpClientService
{
    public interface ICacheHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, string token, HttpContent content);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}
