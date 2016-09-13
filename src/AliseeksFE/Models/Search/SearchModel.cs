using System;
using System.Collections.Generic;
using System.Linq;
using AliseeksFE.Models.Api;

namespace AliseeksFE.Models.Search
{
    public class SearchModel
    {
        public SearchCriteria Criteria { get; set; }
        public SearchResultsModel Results { get; set; }
    }
}