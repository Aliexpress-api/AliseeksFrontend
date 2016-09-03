using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AliseeksFE.Models.Search
{
    public class SearchCriteria
    {
        [Required(ErrorMessage = "Please enter a search")]
        public string SearchText { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public double? PriceFrom { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive price")]
        public double? PriceTo { get; set; }

        public string ShipFrom { get; set; }
        public string ShipTo { get; set; }

        public bool FreeShipping { get; set; }
        public bool SaleItems { get; set; }
        public bool PieceOnly { get; set; }
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
        }
    }
}