using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AliseeksFE.Models.Feedback;
using AliseeksFE.Services.Api;
using Newtonsoft.Json;

namespace AliseeksFE.Services.Feedback
{
    public class FeedbackService : IFeedbackService
    {
        IApiService api;

        public FeedbackService(IApiService api)
        {
            this.api = api;
        }

        public async Task<HttpResponseMessage> Submit(FeedbackModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await api.Post(ApiEndpoints.Feedback, content);
        }
    }
}
