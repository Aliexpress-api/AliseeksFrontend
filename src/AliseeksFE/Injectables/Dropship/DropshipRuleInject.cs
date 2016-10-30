﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Dropship;

namespace AliseeksFE.Injectables.Dropship
{
    public class DropshipRuleInject
    {
        public string PriceAdjustment(DropshipListingRules rules)
        {
            return rules.Price.ToString();
            /*
            if(rules.FixedPricePercentage.HasValue)
            {
                return (rules.FixedPricePercentage.Value * 100).ToString() + "%";
            }

            if(rules.FixedPriceAdjustment.HasValue)
            {
                var unit = (rules.FixedPriceAdjustment.Value > 0) ? "+" : "";

                return unit + rules.FixedPriceAdjustment.ToString();
            }

            if(rules.FixedPrice.HasValue)
            {
                return rules.FixedPrice.ToString();
            } */
        }

        public string StockAdjustment(DropshipListingRules rules)
        {
            return rules.Stock.ToString();
/*
            if(rules.FixedStockAdjustment.HasValue)
            {
                var unit = (rules.FixedStockAdjustment.Value > 0) ? "+" : "";

                return unit + rules.FixedStockAdjustment.ToString();
            }

            if(rules.FixedStock.HasValue)
            {
                return rules.FixedStock.ToString();
            }
             */
        }
    }
}
