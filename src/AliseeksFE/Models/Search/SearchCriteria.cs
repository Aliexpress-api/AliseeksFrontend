using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using AliseeksFE.Injectables.Account;

namespace AliseeksFE.Models.Search
{
    public class SearchCriteria
    {
        [Required(ErrorMessage = "Please enter a search")]
        //[SearchValue("", "Search Text")]
        public string SearchText { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        [SearchValue(0, "Price From")]
        public double? PriceFrom { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        [SearchValue(0, "Price To")]
        public double? PriceTo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive quantity")]
        [SearchValue(0, "Quantity Min")]
        public int? QuantityMin { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Please enter a positive quantity")]
        [SearchValue(0, "Quantity Max")]
        public int? QuantityMax { get; set; }

        [SearchValue("", "Ship From")]
        public string ShipFrom { get; set; }

        [SearchValue("", "Ship To")]
        public string ShipTo { get; set; }

        [SearchValue(false,"Free Shipping")]
        public bool FreeShipping { get; set; }

        [SearchValue(false, "Sale Items")]
        public bool SaleItems { get; set; }

        [SearchValue(false, "Single Piece Only")]
        public bool PieceOnly { get; set; }

        [SearchValue(false, "Mobile Deals Only")]
        public bool AppOnly { get; set; }

        public int Page { get; set; }

        //Not implemented
        public bool UnitPrice { get; set; }
        public bool NeedQuery { get; set; }
        public bool Grid { get; set; }

        public SearchCriteria()
        {
            UnitPrice = true;
            NeedQuery = true;
            Grid = true;
            Page = 1;
        }
    }
}