using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AliseeksFE.Services.Api
{
    public class ApiService : IApiService
    {
        string apiAddress = "http://localhost:1450/";
        HttpContext context;

        public ApiService(IHttpContextAccessor accessor)
        {
            context = accessor.HttpContext;
        }

        public async Task<HttpResponseMessage> Get(string endpoint)
        { 
            HttpResponseMessage response;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiAddress);
                appendAuthorization(client.DefaultRequestHeaders);

                response = await client.GetAsync(endpoint);
            }

            return response;
        }

        public async Task<HttpResponseMessage> Post(string endpoint, StringContent data)
        {
            HttpResponseMessage response;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiAddress);
                appendAuthorization(client.DefaultRequestHeaders);

                response = await client.PostAsync(endpoint, data);
            }

            return response;
        }

        void appendAuthorization(HttpRequestHeaders headers)
        {
            if (!context.User.Identity.IsAuthenticated) { return; }
            var claim = context.User.FindFirst("Token");
            if(claim != null)
            {
                headers.Authorization = new AuthenticationHeaderValue("Bearer", claim.Value);
            }
        }
    }
}
