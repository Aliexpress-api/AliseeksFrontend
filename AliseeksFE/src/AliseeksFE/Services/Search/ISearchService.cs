using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Api;
using AliseeksFE.Models.Search;

namespace AliseeksFE.Services.Search
{
    public interface ISearchService
    {
        Task<IEnumerable<Item>> Search(SearchCriteria criteria);
    }
}
