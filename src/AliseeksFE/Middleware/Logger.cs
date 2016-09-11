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
        private ILogger<Logger> logger;
        private ILoggingService appLogging;

        public Logger(RequestDelegate next, ILogger<Logger> logger, ILoggingService appLogging)
        {
            this._next = next;
            this.logger = logger;
            this.appLogging = appLogging;
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation($"{context.Request.Path}\tRECEIVED");
            var sw = new Stopwatch();
            sw.Start();

            var activity = new LoggingActivityModel()
            {
                IP = context.Connection.RemoteIpAddress.ToString(),
                User = (context.User.Identity.IsAuthenticated == false) ? "Guest" : context.User.FindFirst(ClaimTypes.Name).Value,
                Request = context.Request.Path
            };
            AppTask.Forget(() => appLogging.LogActivity(activity));

            await _next.Invoke(context);
            sw.Stop();
            logger.LogInformation($"{context.Request.Path}\t{sw.Elapsed.TotalMilliseconds}(ms)");
        }
    }
}
