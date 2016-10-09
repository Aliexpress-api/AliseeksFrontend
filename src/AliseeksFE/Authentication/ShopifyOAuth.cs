using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Configuration.Options;
using Microsoft.Extensions.Options;

namespace AliseeksFE.Authentication
{
    public class ShopifyOAuth
    {
        public string GetUrl(string shop)
        {
            var api_key = config.APIKey;
            var scopes = "write_orders,write_customers";
            var redirect_uri = "http://www.aliseeks.com/";
            var nonce = Guid.NewGuid().ToString("N");

            return $"https://{shop}.myshopify.com/admin/oauth/authorize?client_id={api_key}&scope={scopes}&redirect_uri={redirect_uri}&state={nonce}";
        }

        private readonly ShopifyOptions config;

        public ShopifyOAuth(IOptions<ShopifyOptions> config)
        {
            this.config = config.Value;
        }
    }
}
