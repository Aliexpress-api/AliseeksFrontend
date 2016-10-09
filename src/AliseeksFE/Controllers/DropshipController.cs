using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AliseeksFE.Services.Dropshipping;
using SharpRaven.Core;
using AliseeksFE.Models.Search;
using AliseeksFE.Models.Shopify;
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

        public async Task<IActionResult> Integrations()
        {
            var integrations = await dropship.GetIntegrations();

            return View(integrations);
        }

        [HttpGet]
        [Route("[controller]/integrations/shopify")]
        public IActionResult IntegrateShopify()
        {
            return View("ShopifyIntegration");
        }

        [HttpPost]
        [Route("[controller]/integrations/shopify")]
        public async Task<IActionResult> IntegrateShopify(string shop)
        {
            var oauthRequest = await dropship.GetShopifyOAuth(shop);

            if (oauthRequest == null)
            {
                ModelState.AddModelError("OAuth Failed", "Something failed requesting OAuth info from API");
                return View("ShopifyIntegration");
            }
            else
            {
                return Redirect(oauthRequest.Uri);
            }
        }

        [HttpGet]
        [Route("[controller]/integrations/shopify/oauth")]
        public async Task<IActionResult> IntegrateShopifyOAuth([FromQuery]ShopifyOAuthResponse response)
        {
            response.Query = HttpContext.Request.QueryString.Value.Substring(1); //remove leading ?

            await dropship.CompleteShopifyOAuth(response);

            return RedirectToAction("Integrations");
        }
    }
}
