using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Models.Api
{
    public class Item
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public decimal[] Price { get; set; }
        public string Unit { get; set; }
        public bool FreeShipping { get; set; }
        public string ImageURL { get; set; }
        public string MobileOnly { get; set; }
        public string StoreName { get; set; }
        public int Feedback { get; set; }
        public int Orders { get; set; }
        public decimal LotPrice { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
        public string Source { get; set; }
    }
}
