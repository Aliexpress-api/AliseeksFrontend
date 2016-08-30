using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE.Models.Api;
using AliseeksFE.Models.Search;
using AliseeksFE.Services.Api;
using AliseeksFE.Utility;
using Newtonsoft.Json;
using System.Net.Http;

namespace AliseeksFE.Services.Search
{
    public class SearchService : ISearchService
    {
        IApiService api;

        public SearchService(IApiService api)
        {
            this.api = api;
        }

        public async Task<IEnumerable<Item>> Search(SearchCriteria criteria)
        {
            string qs = new QueryStringEncoder().Encode(criteria);
            string endpoint = ApiEndpoints.Search + $"?{qs}";

            var response = await api.Get(endpoint);

            var items = JsonConvert.DeserializeObject<Item[]>(await response.Content.ReadAsStringAsync());

            return items;
        }

        public async Task<HttpResponseMessage> SearchCache(SearchCriteria criteria)
        {
            string qs = new QueryStringEncoder().Encode(criteria);
            string endpoint = ApiEndpoints.SearchCache + $"?{qs}";

            var response = await api.Get(endpoint);

            return response;
        }
    }
}
