using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AliseeksFE.Models.Feedback;
using Microsoft.AspNetCore.Http;
using AliseeksFE.IntegrationTests.Utilities;
using System.Net.Http;

namespace AliseeksFE.IntegrationTests.Controllers
{
    public class FeedbackControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _client;

        public FeedbackControllerTests(TestServerFixture client)
        {
            _client = client;
        }

        [Fact]
        public async Task CanReachFeedback()
        {
            var response = await _client.GetAsync(Endpoints.Feedback);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task CanPostFeedback()
        {
            var model = new FeedbackModel()
            {
                Email = "test@test.com",
                Message = "test feedback"
            };

            var content = new FormUrlEncodedContent(model.ToKeyValue());

            var response = await _client.PostAsync(Endpoints.Feedback, content);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Redirect);
        }
    }
}
