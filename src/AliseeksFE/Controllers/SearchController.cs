using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Services.Search;
using AliseeksFE.Models.Search;
using AliseeksFE.Services.Api;
using AliseeksFE.Filters;
using Microsoft.AspNetCore.Http;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using AliseeksFE.Utility.Extensions;
using AliseeksFE.Services;
using Newtonsoft.Json;
using AliseeksFE.Models.Api;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    public class SearchController : Controller
    {
        ISearchService search;
        IRavenClient raven;
        IApiService api;

        public SearchController(ISearchService search, IApiService api, IRavenClient raven)
        {
            this.search = search;
            this.raven = raven;
            this.api = api;
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

                return Ok();
            }
            else
            {
                return LocalRedirect("/");
            }
        }

        [HttpGet]
        [Route("[controller]/ajax/aliexpress")]
        public async Task<IActionResult> AjaxSingleItem(string link)
        {
            var response = await api.Get(ApiEndpoints.SearchSingle(link, "Aliexpress"));

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ItemDetail>(json);
                if (product == null)
                    return NotFound();

                return Json(product);
            }

            return NotFound();
        }

        [Route("/[controller]/save/{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await search.DeleteSearch(id);

            return RedirectToAction("Index", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Cache(SearchCriteria criteria)
        {
            var response = await search.SearchCache(criteria);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PriceHistory([FromBody]PriceHistoryRequestModel[] models)
        {
            var response = await search.PriceHistory(models);

            return Ok();
        }
    }
}
