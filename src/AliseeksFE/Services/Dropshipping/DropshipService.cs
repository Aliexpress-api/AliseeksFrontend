using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Services.Api;
using AliseeksFE.Models.Search;
using AliseeksFE.Models.Dropship;
using AliseeksFE.Utility;
using Newtonsoft.Json;

namespace AliseeksFE.Services.Dropshipping
{
    public class DropshipService
    {
        private readonly IApiService api;

        public DropshipService(IApiService api)
        {
            this.api = api;
        }

        public async Task AddProduct(SingleItemRequest item)
        {
            var json = JsonConvert.SerializeObject(item);
            var jsonContent = new JsonContent(json);

            var response = await api.Post(ApiEndpoints.DropshipAddProduct, jsonContent);
        }

        public async Task UpdateProduct(DropshipItemModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var jsonContent = new JsonContent(json);

            var response = await api.Post(ApiEndpoints.DropshipUpdateProduct, jsonContent);
        }

        public async Task<DropshipItem[]> GetProducts()
        {
            var response = await api.Get(ApiEndpoints.DropshipGetProducts);

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<DropshipItem[]>(json);
                return products;
            }

            return new DropshipItem[0];
        }

        public async Task<DropshipOverview> GetOverview()
        {
            var response = await api.Get(ApiEndpoints.DropshipOverview);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var overview = JsonConvert.DeserializeObject<DropshipOverview>(json);
                return overview;
            }

            return new DropshipOverview();
        }

        public async Task<DropshipAccount> GetAccount()
        {
            var response = await api.Get(ApiEndpoints.DropshipGetAccount);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var account = JsonConvert.DeserializeObject<DropshipAccount>(json);
                return account;
            }

            return new DropshipAccount()
            {
                Status = AccountStatus.New
            };
        }
        
        public async Task SetupAccount(DropshipAccountConfiguration config)
        {
            var json = JsonConvert.SerializeObject(config);
            var jsonContent = new JsonContent(json);

            var response = await api.Post(ApiEndpoints.DropshipCreateAccount, jsonContent);
        }
    }
}
