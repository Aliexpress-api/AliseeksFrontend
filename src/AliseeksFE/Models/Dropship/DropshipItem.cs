using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Shopify;

namespace AliseeksFE.Models.Dropship
{
    public class DropshipItem
    {
        public DropshipItemModel Dropshipping { get; set; }
        public ShopifyProductModel Product { get; set; }
    }
}
