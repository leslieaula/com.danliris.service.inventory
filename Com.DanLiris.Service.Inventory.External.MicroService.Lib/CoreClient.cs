using Com.DanLiris.Service.Inventory.External.MicroService.Lib.Cache;
using Com.DanLiris.Service.Inventory.External.MicroService.Lib.HttpClientService;
using Com.DanLiris.Service.Inventory.External.MicroService.Lib.MongoMasterResult;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

namespace Com.DanLiris.Service.Inventory.External.MicroService.Lib
{
    public class CoreClient : ICoreClient
    {
        private readonly ICacheHttpClientService _httpClientService;
        private readonly IMemoryCacheManager _cacheManager;

        public CoreClient(ICacheHttpClientService httpClientService, IServiceProvider serviceProvider)
        {
            _httpClientService = httpClientService;
            _cacheManager = serviceProvider.GetService<IMemoryCacheManager>();
        }

        public void SetProduct()
        {
            var masterProductUri = MasterDataSettings.Endpoint + $"master/products/simple";
            var productResponse = _httpClientService.GetAsync(masterProductUri).Result;

            var productResult = new ProductResult();
            if (productResponse.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                productResult = JsonConvert.DeserializeObject<ProductResult>(productResponse.Content.ReadAsStringAsync().Result);
            }
            _cacheManager.Set("Products", productResult.data);
        }

        public void SetStorage()
        {
            var uri = MasterDataSettings.Endpoint + $"master/storages?size={int.MaxValue}";
            var response = _httpClientService.GetAsync(uri).Result;

            var result = new StorageResult();
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<StorageResult>(response.Content.ReadAsStringAsync().Result);
            }
            _cacheManager.Set("Storages", result.data);
        }

        public void SetUom()
        {
            var uri = MasterDataSettings.Endpoint + $"master/uoms?size={int.MaxValue}";
            var response = _httpClientService.GetAsync(uri).Result;

            var result = new UomResult();
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<UomResult>(response.Content.ReadAsStringAsync().Result);
            }
            _cacheManager.Set("Uoms", result.data);
        }
    }
}
