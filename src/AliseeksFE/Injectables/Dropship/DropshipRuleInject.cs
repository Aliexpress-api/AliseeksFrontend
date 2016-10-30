using System;
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
            var element = "";
            switch(rules.PriceRule)
            {
                case PriceRule.FixedPrice:
                    break;

                case PriceRule.PriceAdjustment:
                    if(rules.Price > 0)
                    {
                        element += $"<i class=\"fa fa-angle-up\" aria-hidden=\"true\"></i> $ {rules.Price}";
                    }
                    else
                    {
                        element += $"<i class=\"fa fa-angle-down\" aria-hidden=\"true\"></i> $ {rules.Price}";
                    }
                    break;

                case PriceRule.PricePercentage:
                    element += $"{rules.Price} %";
                    break;

                case PriceRule.None:
                    element += "None";
                    break;
            }

            return element;
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
            string element = "";

            switch(rules.StockRule)
            {
                case StockRule.FixedStock:
                    element += $"{rules.Stock} Fixed";
                    break;

                case StockRule.StockAdjustment:
                    if (rules.Stock > 0)
                    {
                        element += $"<i class=\"fa fa-angle-down\" aria-hidden=\"true\"></i> $ {rules.Stock}";
                    }
                    else
                    {
                        element += $"<i class=\"fa fa-angle-up\" aria-hidden=\"true\"></i> $ {rules.Stock}";
                    }
                    break;
            }

            return element;
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
