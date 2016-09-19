using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpRaven.Core;
using SharpRaven.Core.Data;

namespace AliseeksFE.Filters
{
    public class ModelBinderBreadcrumbFilter : IAsyncResourceFilter
    {
        IRavenClient raven;

        public ModelBinderBreadcrumbFilter(IRavenClient raven)
        {
            this.raven = raven;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {

            var bindingCrumb = new Breadcrumb("ModelBinderBreadcrumb");
            bindingCrumb.Level = BreadcrumbLevel.Info;
            bindingCrumb.Data = new Dictionary<string, string>();
            bindingCrumb.Data.Add("QueryString", context.HttpContext.Request.QueryString.ToUriComponent());

            raven.AddTrail(bindingCrumb);

            await next();

            var bindingAfterCrumb = new Breadcrumb("ModelBinderBreadcrumb");
            bindingCrumb.Level = BreadcrumbLevel.Info;
            bindingCrumb.Data = new Dictionary<string, string>();
            bindingCrumb.Data.Add("ModelState", context.ModelState.IsValid.ToString());

            raven.AddTrail(bindingAfterCrumb);

        }
    }
}
