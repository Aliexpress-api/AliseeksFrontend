using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Api;
using AliseeksFE.Models.Search;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace AliseeksFE.Services.Search
{
    public interface ISearchService
    {
        Task<SearchResultsModel> Search(SearchCriteria criteria);
        Task<HttpResponseMessage> SearchCache(SearchCriteria criteria);
        Task<HttpResponseMessage> Save(SearchCriteria criteria);
        Task<HttpResponseMessage> DeleteSearch(int id);
        Task<HttpResponseMessage> PriceHistory(PriceHistoryRequestModel[] models);
    }
}
