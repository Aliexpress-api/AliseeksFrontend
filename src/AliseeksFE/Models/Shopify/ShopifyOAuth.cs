using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Models.Shopify
{
    public class ShopifyOAuthRequest
    {
        public string Shop { get; set; }
        public string APIKey { get; set; }
        public string Scopes { get; set; }
        public string RedirectUri { get; set; }
        public string Nounce { get; set; }
        public string Uri { get; set; }
    }    

    public class ShopifyOAuthResponse
    {
        public string Code { get; set; }
        public string Hmac { get; set; }
        public string Shop { get; set; }
        public string State { get; set; }
        public string Query { get; set; }
    }
}
