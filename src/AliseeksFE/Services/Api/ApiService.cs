using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AliseeksFE.Services.Api
{
    public class ApiService : IApiService
    {
        string apiAddress = "http://localhost:1450/";

        public async Task<HttpResponseMessage> Get(string endpoint)
        {
            HttpResponseMessage response;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiAddress);
                response = await client.GetAsync(endpoint);
            }

            return response;
        }

        public async Task<HttpResponseMessage> Post(string endpoint, StringContent data)
        {
            HttpResponseMessage response;

            using (HttpClient client = new HttpClient())
            {
                response = await client.PostAsync(endpoint, data);
            }

            return response;
        }
    }
}
