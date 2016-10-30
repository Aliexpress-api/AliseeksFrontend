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
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.Extensions.Options;
using AliseeksFE.Configuration.Options;

namespace AliseeksFE.Services.Dropshipping
{
    public class DropshipService
    {
        private readonly IApiService api;
        private readonly ShopifyOptions shopifyConfig;

        public DropshipService(IApiService api, IOptions<ShopifyOptions> shopifyConfig)
        {
            this.api = api;
            this.shopifyConfig = shopifyConfig.Value;
        }

        public async Task<HttpResponseMessage> AddProduct(SingleItemRequest item)
        {
            var json = JsonConvert.SerializeObject(item);
            var jsonContent = new JsonContent(json);

            var response = await api.Post(ApiEndpoints.DropshipAddProduct, jsonContent);

            return response;
        }

        public async Task UpdateProduct(DropshipItemModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var jsonContent = new JsonContent(json);

            var response = await api.Post(ApiEndpoints.DropshipUpdateProduct, jsonContent);
        }

        public async Task<DropshipItem> GetProduct(int itemid)
        {
            var response = await api.Get(ApiEndpoints.DropshipGetProduct(itemid));

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<DropshipItem>(json);
                return product;
            }

            return new DropshipItem();
        }

        public async Task<DropshipItem[]> GetProducts()
        {
            var response = await api.Get(ApiEndpoints.DropshipGetProducts(0, 28));

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

        public async Task DeleteIntegration(int id)
        {
            var response = await api.Delete(ApiEndpoints.DropshipIntegrationDelete(id));
        }

        public async Task<ShopifyOAuthRequest> GetShopifyOAuth(string shop)
        {
            var qs = new Dictionary<string, string>()
            {
                { "shop", shop },
                { "redirect", shopifyConfig.OAuthRedirect }
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
