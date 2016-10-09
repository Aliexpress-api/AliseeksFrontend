using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AliseeksFE.Services.Dropshipping;
using SharpRaven.Core;
using AliseeksFE.Models.Search;
using AliseeksFE.Filters;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    [Authorize]
    [TypeFilter(typeof(DropshipAuthorizationFilter))]
    public class DropshipController : Controller
    {
        private readonly DropshipService dropship;
        private readonly IRavenClient raven;

        public DropshipController(DropshipService dropship, IRavenClient raven)
        {
            this.dropship = dropship;
            this.raven = raven;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var overview = await dropship.GetOverview();

            return View(overview);
        }

        public async Task<IActionResult> Add(SingleItemRequest item)
        {
            await dropship.AddProduct(item);

            return Ok();
        }
    }
}
