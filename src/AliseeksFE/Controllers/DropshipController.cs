﻿using System;
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
using AliseeksFE.Services.Api;
using Newtonsoft.Json;
using AliseeksFE.Services;
using AliseeksFE.Models.Dropship;
using AliseeksFE.Models.Api;
using AliseeksFE.Utility;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    [Authorize]
    [TypeFilter(typeof(DropshipAuthorizationFilter))]
    public class DropshipController : Controller
    {
        private readonly DropshipService dropship;
        private readonly IRavenClient raven;
        private readonly IApiService api;

        private const int productsPerPage = 20;

        public DropshipController(DropshipService dropship, IApiService api, IRavenClient raven)
        {
            this.dropship = dropship;
            this.raven = raven;
            this.api = api;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var overview = await dropship.GetOverview();

            return View(overview);
        }

        public async Task<IActionResult> Add(SingleItemRequest item)
        {
            var response = await dropship.AddProduct(item);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return RedirectToAction("Integrations");

            return RedirectToAction("Products");
        }

        [HttpGet]
        [Route("[controller]/products/add")]
        public IActionResult AddProduct()
        {
            return View(new DropshipItemModel()
            {
                Source = new SingleItemRequest()
                {
                    Source = "Aliexpress"
                },
                Rules = DropshipListingRules.Default
            });
        }

        [HttpPost]
        [Route("[controller]/products/add")]
        public async Task<IActionResult> AddProduct(DropshipItemModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await dropship.AddProduct(model);

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("All", await response.Content.ReadAsStringAsync());
                return View(model);
            }

            return RedirectToAction("Products");
        }

        [HttpGet]
        [Route("[controller]/products/{itemid}")]
        public async Task<IActionResult> Product(int itemid)
        {
            var dropshipItem = await dropship.GetProduct(itemid);

            return View(dropshipItem.Dropshipping);
        }

        [HttpGet]
        [Route("[controller]/products/{itemid}/add")]
        public async Task<IActionResult> ProductAddToIntegration(int itemid)
        {
            var response = await api.Get(ApiEndpoints.DropshipAddProductIntegration(itemid));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Products");
            }
            else
            {
                return RedirectToAction("Products");
            }
        }

        [HttpPost]
        [Route("[controller]/products/{itemid}")]
        public async Task<IActionResult> Product(int itemid, [FromForm]DropshipItemModel item)
        {
            item.ID = itemid;

            var json = JsonConvert.SerializeObject(item);
            var response = await api.Put(ApiEndpoints.DropshipUpdateProduct, new JsonContent(json));

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Products");
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("All", message);
                return View(item);
            }
        }

        [Route("[controller]/products/{itemid}/delete")]
        public async Task<IActionResult> ProductDelete(int itemid)
        {
            var response = await api.Delete(ApiEndpoints.DropshipProductDelete(itemid));

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Products");
            }
            else
            {
                return RedirectToAction("Products");
            }
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

        [Route("[controller]/integration/{id}/delete")]
        public async Task<IActionResult> DeleteIntegration(int id)
        {
            var username = HttpContext.User.Identity.Name;

            await dropship.DeleteIntegration(id);

            return RedirectToAction("Integrations");
        }

        [HttpGet]
        [Route("[controller]/integrations/shopify/oauth")]
        public async Task<IActionResult> IntegrateShopifyOAuth([FromQuery]ShopifyOAuthResponse response)
        {
            response.Query = HttpContext.Request.QueryString.Value.Substring(1); //remove leading ?

            await dropship.CompleteShopifyOAuth(response);

            return RedirectToAction("Integrations");
        }

        [HttpGet]
        [Route("[controller]/ajax/aliexpress")]
        public async Task<IActionResult> AjaxAliexpressVerify(string link)
        {
            var response = await api.Get(ApiEndpoints.SearchSingle(link, "Aliexpress"));

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ItemDetail>(json);
                if (product == null)
                    return NotFound();

                return PartialView("Partials/AliexpressVerify", product);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("[controller]/ajax/shopify")]
        public async Task<IActionResult> AjaxShopifyVerify(string listingid)
        {
            var response = await api.Get(ApiEndpoints.DropshipIntegrationGetProduct("Shopify", listingid));

            if(response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ShopifyProductModel>(json);
                return PartialView("Partials/ShopifyVerify", product);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Products(int page = 1)
        {
            var response = await api.Get(ApiEndpoints.DropshipGetProducts(productsPerPage * (page - 1), productsPerPage));

            var products = new DropshipItem[0];

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<DropshipItem[]>(json);
            }

            //Add total count for pagination
            ViewBag.MaxCount = response.Headers.Contains("X-Total-Count") ? Convert.ToInt32(response.Headers.GetValues("X-Total-Count").First()) : productsPerPage;
            ViewBag.Page = page;
            ViewBag.ItemsPerPage = productsPerPage;

            return View(products);
        }
    }
}
