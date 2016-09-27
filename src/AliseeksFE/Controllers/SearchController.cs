using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Services.Search;
using AliseeksFE.Models.Search;
using AliseeksFE.Filters;
using Microsoft.AspNetCore.Http;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using AliseeksFE.Utility.Extensions;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    public class SearchController : Controller
    {
        ISearchService search;
        IRavenClient raven;

        public SearchController(ISearchService search, IRavenClient raven)
        {
            this.search = search;
            this.raven = raven;
        }

        // POST: /search
        [HttpGet]
        [Route("/search")]
        [ServiceFilter(typeof(ModelBinderBreadcrumbFilter))]
        public async Task<IActionResult> Search(SearchCriteria criteria)
        {
            if(ModelState.IsValid)
            {
                var results = await search.Search(criteria);

                var model = new SearchModel() { Criteria = criteria, Results = results };

                return View(model);
            }
            else
            {
                var message = new SentryEvent(new SentryMessage("Invalid Search Criteria"));
                message.Level = ErrorLevel.Warning;
                await raven.CaptureAsync(message);

                return LocalRedirect("/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Save(SearchCriteria criteria)
        {
            if(ModelState.IsValid)
            {
                await search.Save(criteria);

                return await Search(criteria);
            }
            else
            {
                return LocalRedirect("/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Cache(SearchCriteria criteria)
        {
            var response = await search.SearchCache(criteria);
            return Ok();
        }
    }
}
