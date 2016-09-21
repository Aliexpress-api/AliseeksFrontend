using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using System.Net.Http;
using AliseeksFE;
using System.IO;

namespace AliseeksFE.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public HomeControllerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>());

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task CanReachIndex()
        {
            var response = await _client.GetAsync(Endpoints.Index);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
