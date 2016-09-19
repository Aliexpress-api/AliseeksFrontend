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
using System.Net;

namespace AliseeksFE.Services.Search
{
    public class SearchService : ISearchService
    {
        IApiService api;

        public SearchService(IApiService api)
        {
            this.api = api;
        }

        public async Task<SearchResultsModel> Search(SearchCriteria criteria)
        {
            string qs = new QueryStringEncoder().Encode(criteria);
            string endpoint = ApiEndpoints.Search + $"?{qs}";

            var response = await api.Get(endpoint);

            var model = new SearchResultsModel();

            switch(response.StatusCode)
            {
                case HttpStatusCode.OK:
                    model.SearchCount = response.Headers.Contains("X-TOTAL-COUNT") ? int.Parse(response.Headers.First(x => x.Key == "X-TOTAL-COUNT").Value.First()) : 0;
                    model.Items = JsonConvert.DeserializeObject<Item[]>(await response.Content.ReadAsStringAsync());
                    break;
            }

            return model;
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
