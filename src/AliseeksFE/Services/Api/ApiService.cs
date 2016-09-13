using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AliseeksFE.Configuration.Options;

namespace AliseeksFE.Services.Api
{
    public class ApiService : IApiService
    {
        string apiAddress = "http://localhost:1450/";
        HttpContext context;
        ILogger<ApiService> logger;

        public ApiService(IHttpContextAccessor accessor, ILogger<ApiService> logger, IOptions<ApiOptions> config)
        {
            context = accessor.HttpContext;
            this.logger = logger;
            apiAddress = config.Value.ApiAddress;
        }

        public async Task<HttpResponseMessage> Get(string endpoint)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiAddress);
                appendAuthorization(client.DefaultRequestHeaders);

                try
                {
                    response = await client.GetAsync(endpoint);
                }
                catch(HttpRequestException e)
                {
                    logger.LogCritical(new EventId(502), e, $"Aliseeks API Service is unavailable at {apiAddress + endpoint}");
                }
                catch(Exception e)
                {
                    logger.LogError(new EventId(503), e, $"Unknown Aliseeks API Service error when requesting {apiAddress + endpoint}");
                }
            }

            return response;
        }

        public async Task<HttpResponseMessage> Post(string endpoint, StringContent data)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiAddress);
                appendAuthorization(client.DefaultRequestHeaders);

                try
                {
                    response = await client.PostAsync(endpoint, data);
                }
                catch (HttpRequestException e)
                {
                    logger.LogCritical(new EventId(502), e, $"Aliseeks API Service is unavailable at {apiAddress + endpoint}");
                }
                catch (Exception e)
                {
                    logger.LogError(new EventId(503), e, $"Unknown Aliseeks API Service error when requesting {apiAddress + endpoint}");
                }
            }

            return response;
        }

        void appendAuthorization(HttpRequestHeaders headers)
        {
            if (context == null || !context.User.Identity.IsAuthenticated) { return; }
            var claim = context.User.FindFirst("Token");
            if (claim != null)
            {
                headers.Authorization = new AuthenticationHeaderValue("Bearer", claim.Value);
            }
        }
    }
}
