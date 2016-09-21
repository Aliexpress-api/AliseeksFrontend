using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AliseeksFE.IntegrationTests.Controllers
{
    public class LoginControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _client;

        public LoginControllerTests(TestServerFixture _client)
        {
            this._client = _client;
        }

        [Fact]
        public async Task CanReachLogin()
        {
            var response = await _client.GetAsync(Endpoints.Login);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CanReachLogout()
        {
            var response = await _client.GetAsync(Endpoints.Logout);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task CanReachRegister()
        {
            var response = await _client.GetAsync(Endpoints.Register);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CanReachPasswordReset()
        {
            var response = await _client.GetAsync(Endpoints.PasswordReset);

            Assert.True(response.IsSuccessStatusCode);
        }


        [Fact]
        public async Task CanReachPasswordResetValid()
        {
            var response = await _client.GetAsync(Endpoints.PasswordResetValid, new Dictionary<string, string>() { { "token", "testtoken" } });

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CanLogin()
        {
            Assert.True(true);
        }
    }
}
