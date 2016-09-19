﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Services.Search;
using AliseeksFE.Models.Search;
using AliseeksFE.Filters;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    public class SearchController : Controller
    {
        ISearchService search;

        public SearchController(ISearchService search)
        {
            this.search = search;
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
