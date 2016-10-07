﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliseeksFE.Models.Dropship
{
    public class DropshipListingRules
    {
        public static DropshipListingRules Default
        {
            get
            {
                return new DropshipListingRules()
                {
                    FixedPricePercentage = 0.10m,
                    FixedPriceAdjustment = 10,
                    FixedStockAdjustment = -5
                };
            }
        }

        public decimal? FixedPrice { get; set; }
        public decimal? FixedPriceAdjustment { get; set; }
        public decimal? FixedPricePercentage { get; set; }
        public int? FixedStock { get; set; }
        public int? FixedStockAdjustment { get; set; }
    }
}
