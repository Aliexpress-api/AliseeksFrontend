using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Api;

namespace AliseeksFE.Models.Search
{
    public class SearchResultsModel
    {
        public int SearchCount { get; set; }
        public Item[] Items { get; set; }

        public SearchResultsModel()
        {
            SearchCount = 0;
            Items = new Item[0];
        }
    }
}
