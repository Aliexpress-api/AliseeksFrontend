using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using AliseeksFE.Services.Logging;
using AliseeksFE.Models.Logging;
using System.Security.Claims;
using AliseeksFE.Utility.Extensions;
using SharpRaven.Core;
using SharpRaven.Core.Logging;
using SharpRaven.Core.Data;
using AliseeksFE.Features;

namespace AliseeksFE.Middleware
{
    public static class LoggerExtension
    {
        public static IApplicationBuilder ApplyApiLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<Logger>();
            return app;
        }
    }

    public class Logger
    {
        private readonly RequestDelegate _next;
        private readonly IRavenClient raven;
        private ILogger<Logger> logger;
        private ILoggingService appLogging;

        public Logger(RequestDelegate next, ILogger<Logger> logger, ILoggingService appLogging, IRavenClient raven)
        {
            this._next = next;
            this.logger = logger;
            this.appLogging = appLogging;
            this.raven = raven;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                logger.LogInformation($"{context.Request.Path}\tRECEIVED");
                var sw = new Stopwatch();
                sw.Start();

                //Log activty through Sentry Breadcrumb
                var crumb = new Breadcrumb("LoggerMiddleware");
                crumb.Message = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString.ToUriComponent()}";
                crumb.Data = new Dictionary<string, string>() {
                { "IsAuthenticated", context.User.Identity.IsAuthenticated.ToString() },
                { "Authentication", context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Unknown" }
                    };
                raven.AddTrail(crumb);

                //Log activty through API
                var activity = new LoggingActivityModel()
                {
                    IP = context.Connection.RemoteIpAddress.ToString(),
                    User = (context.User.Identity.IsAuthenticated == false) ? "Guest" : context.User.FindFirst(ClaimTypes.Name).Value,
                    Request = context.Request.Path
                };
                AppTask.Forget(() => appLogging.LogActivity(activity));

                //Start processing the request
                try
                {
                    await _next.Invoke(context);
                }
                catch (Exception e)
                {
                    //Allow Postgres to receive querystring
                    var feature = new LoggerFeature()
                    {
                        Path = context.Request.Path + context.Request.QueryString.ToString()
                    };
                    context.Features.Set<LoggerFeature>(feature);

                    //Try to send the request to Sentry
                    await raven.CaptureNetCoreEventAsync(e);

                    //rethrow so we can redirect to Error page
                    throw e;
                }

                //Stop request timing and output time
                sw.Stop();
                logger.LogInformation($"{context.Request.Path}\t{sw.Elapsed.TotalMilliseconds}(ms)");
            }
            catch(Exception e)
            {
                //Try to send the request to Sentry
                await raven.CaptureNetCoreEventAsync(e);
            }
        }
    }
}
