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
using SharpRaven.Core;
using SharpRaven.Core.Data;
using AliseeksFE.Utility.Extensions;
using AliseeksFE.Features;

namespace AliseeksFE.Services.Api
{
    public class ApiService : IApiService
    {
        const string ApiUnavailableMessage = "We apologize for the inconvienance but it seems that our API is currently down.";

        string apiAddress
        {
            get
            {
                return $"http://{address}:{port}/";
            }
        }
        string address = "localhost";
        string port = "1460";
        HttpContext context;
        ILogger<ApiService> logger;

        private readonly IRavenClient raven;

        public ApiService(IHttpContextAccessor accessor,
            ILogger<ApiService> logger,
            IOptions<ApiOptions> config,
            IRavenClient raven)
        {
            context = accessor.HttpContext;
            this.logger = logger;

            address = config.Value.Host;
            port = config.Value.Port;

            this.raven = raven;
        }

        public async Task<HttpResponseMessage> Get(string endpoint, Action<HttpClient> clientConfig = null)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(endpoint, UriKind.Relative));

            response = await sendRequest(request, clientConfig);

            return response;
        }

        public async Task<HttpResponseMessage> Post(string endpoint, StringContent data, Action<HttpClient> clientConfig = null)
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(endpoint, UriKind.Relative));
            request.Content = data;

            response = await sendRequest(request, clientConfig);


            return response;
        }

        async Task preRequest(HttpRequestMessage request)
        {
            //Add authorization to request
            appendAuthorization(request.Headers);

            //Add breadcrumb for sentry tracking
            var crumb = new Breadcrumb("ApiService");
            crumb.Level = BreadcrumbLevel.Info;
            crumb.Message = $"Send {request.Method} {apiAddress}{request.RequestUri.ToString()}";
            crumb.Data = request.Headers.ToDictionary(x => x.Key, v => String.Join(" ", v.Value));

            //Add body if any
            if(request.Content != null)
                crumb.Data.Add("Body", await request.Content.ReadAsStringAsync());        
        }

        async Task postRequest(HttpResponseMessage response)
        {
            var crumb = new Breadcrumb("ApiService");
            crumb.Level = BreadcrumbLevel.Info;
            crumb.Message = $"Received {response.RequestMessage.Method} {apiAddress}{response.RequestMessage.RequestUri.ToString()}";
            crumb.Data = response.Headers.ToDictionary(x => x.Key, v => String.Join(" ", v.Value));

            //Add body if any
            if (response.Content != null)
                crumb.Data.Add("Body", await response.Content.ReadAsStringAsync());
        }

        async Task<HttpResponseMessage> sendRequest(HttpRequestMessage request, Action<HttpClient> clientConfig = null)
        {
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable);
            
            using (HttpClient client = new HttpClient())
            { 
                try
                {
                    await preRequest(request);

                    if (clientConfig != null)
                        clientConfig(client);

                    response = await client.SendAsync(request);
                    await postRequest(response);
                }
                catch (HttpRequestException e)
                {
                    await raven.CaptureNetCoreEventAsync(e);
                    logger.LogCritical(new EventId(502), e, $"Aliseeks API Service is unavailable at {request.RequestUri.ToString()}");
                }
                catch (Exception e)
                {
                    await raven.CaptureNetCoreEventAsync(e);
                    logger.LogError(new EventId(503), e, $"Unknown Aliseeks API Service error when requesting {request.RequestUri.ToString()}");
                }
            }

            //Show message to user when API issues are occuring
            switch(response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadGateway:
                    if (context != null)
                        context.Features.Set<ApplicationMessageFeature>(new ApplicationMessageFeature()
                        {
                            Message = ApiUnavailableMessage,
                            Level = Category.Danger
                        });
                    break;

                case System.Net.HttpStatusCode.BadRequest:
                    if (context != null)
                        context.Features.Set<ApplicationMessageFeature>(new ApplicationMessageFeature()
                        {
                            Message = ApiUnavailableMessage,
                            Level = Category.Danger
                        });
                    break;

                case System.Net.HttpStatusCode.ServiceUnavailable:
                    if (context != null)
                        context.Features.Set<ApplicationMessageFeature>(new ApplicationMessageFeature()
                        {
                            Message = ApiUnavailableMessage,
                            Level = Category.Danger
                        });
                    break;
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
