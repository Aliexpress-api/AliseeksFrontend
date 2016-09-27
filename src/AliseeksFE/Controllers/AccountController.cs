using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AliseeksFE.Services.User;
using AliseeksFE.Models.Account;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService user;

        public AccountController(IUserService user)
        {
            this.user = user;   
        }

        // GET: /<controller>/
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var username = String.Empty;
            if (HttpContext.User.Identity.IsAuthenticated)
                username = HttpContext.User.Identity.Name;

            if (username == String.Empty)
                return Unauthorized();

            var overview = await user.Overview(username);

            var model = new AccountOverview()
            {
                User = overview
            };

            return View(model);
        }
    }
}
