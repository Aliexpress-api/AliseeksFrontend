using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Services.Api;
using AliseeksFE.Models.Search;
using AliseeksFE.Models.Dropship;
using AliseeksFE.Utility;
using Newtonsoft.Json;
using AliseeksFE.Models.Shopify;
using Microsoft.AspNetCore.WebUtilities;

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

        public async Task<DropshipIntegration[]> GetIntegrations()
        {
            var response = await api.Get(ApiEndpoints.DropshipIntegrations);

            var message = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var integrations = JsonConvert.DeserializeObject<DropshipIntegration[]>(message);

                return integrations;
            }
            else
                return new DropshipIntegration[0];
        }

        public async Task<ShopifyOAuthRequest> GetShopifyOAuth(string shop)
        {
            var qs = new Dictionary<string, string>()
            {
                { "shop", shop },
                { "redirect", "http://localhost:1470/dropship/integrations/shopify/oauth" }
            };

            var uri = QueryHelpers.AddQueryString(ApiEndpoints.DropshipOAuthShopify, qs);

            var response = await api.Get(uri);

            var message = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                var oauthRequest = JsonConvert.DeserializeObject<ShopifyOAuthRequest>(message);

                return oauthRequest;
            }

            return null;
        }

        public async Task CompleteShopifyOAuth(ShopifyOAuthResponse resp)
        {
            var json = JsonConvert.SerializeObject(resp);
            var jsonContent = new JsonContent(json);

            var response = await api.Post(ApiEndpoints.DropshipOAuthShopify, jsonContent);                      
        }
    }
}
