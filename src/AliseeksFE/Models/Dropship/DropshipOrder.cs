using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Shopify;

namespace AliseeksFE.Models.Dropship
{
    public class DropshipOrder
    {
        public ShopifyOrder Order { get; set; }
        public DropshipItemModel[] Items { get; set; }
    }
}
