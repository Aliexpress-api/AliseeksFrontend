using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AliseeksFE;

namespace AliseeksFE.IntegrationTests
{
    public class TestServerFixture
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public TestServerFixture()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSetting("ASPNETCORE_ENVIRONMENT", "STAGING")
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        public Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return _client.GetAsync(endpoint);
        }

        public Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            return _client.PostAsync(endpoint, content);
        }

        public Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> qs)
        {
            //Construct query string
            var qsurl = new QueryString();

            foreach(string key in qs.Keys)
            {
                qsurl.Add(key, qs[key]);
            }

            return GetAsync($"{endpoint}{qsurl.ToUriComponent()}");
        }
    }
}
