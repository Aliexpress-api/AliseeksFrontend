using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AliseeksFE.Models.Login;

namespace AliseeksFE.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/feedback")]
        public IActionResult Feedback()
        {
            return View();
        }
    }
}
