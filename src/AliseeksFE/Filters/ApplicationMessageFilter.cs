using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using AliseeksFE.Features;
using Newtonsoft.Json;
using System.Text;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using AliseeksFE.Utility.Extensions;

namespace AliseeksFE.Filters
{
    public class ApplicationMessageFilter : IAsyncActionFilter
    {
        IRavenClient raven;

        public ApplicationMessageFilter(IRavenClient raven)
        {
            this.raven = raven;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ISessionFeature session = null;

            try
            {
                //Any application messages, clear them out
                session = context.HttpContext.Features.Get<ISessionFeature>();
                if (session != null && session.Session.IsAvailable)
                    session.Session.Remove("application-message");
            }
            catch(Exception e)
            {
                await raven.CaptureNetCoreEventAsync(e);
            }

            //Execute the action method
            await next();

            try
            {
                //Any application messages, add to session so they can be retrieved after any redirects
                var messages = context.HttpContext.Features.Get<ApplicationMessageFeature>();
                if (messages != null && session != null && session.Session.IsAvailable)
                    session.Session.Set("application-message", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messages)));
            }
            catch(Exception e)
            {
                await raven.CaptureNetCoreEventAsync(e);
            }
        }
    }
}
