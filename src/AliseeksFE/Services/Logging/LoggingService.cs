using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AliseeksFE.Models.Logging;
using AliseeksFE.Services.Api;
using Newtonsoft.Json;

namespace AliseeksFE.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        IApiService api;

        public LoggingService(IApiService api)
        {
            this.api = api;
        }

        public async Task<HttpResponseMessage> LogException(LoggingExceptionModel model)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return await api.Post(ApiEndpoints.LoggingException, content);
        }
    }
}
