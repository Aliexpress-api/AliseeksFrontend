using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseeksFE;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Xunit;

namespace AliseeksFE.IntegrationTests.Controllers
{
    public class HomeControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _client;

        public HomeControllerTests(TestServerFixture client)
        {
            this._client = client;
        }

        [Fact]
        public async Task CanReachIndex()
        {
            var response = await _client.GetAsync(Endpoints.Index);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CanReachAbout()
        {
            var response = await _client.GetAsync(Endpoints.About);

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
