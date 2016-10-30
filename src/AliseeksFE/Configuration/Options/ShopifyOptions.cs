using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Configuration.Options
{
    public class ShopifyOptions
    {
        public string APIKey { get; set; }
        public string Refresh { get; set; }
        public string Credentials { get; set; }
        public string OAuthRedirect { get; set; }
    }
}
