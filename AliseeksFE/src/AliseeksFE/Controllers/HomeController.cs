using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AliseeksFE.Services.Search;

namespace AliseeksFE.Controllers
{
    public class HomeController : Controller
    {
        ISearchService search;

        public HomeController(ISearchService search)
        {
            this.search = search;
        }

        public IActionResult Index()
        {
            search.Search(new Models.Search.SearchCriteria() { SearchText = "40mm 12" });
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
