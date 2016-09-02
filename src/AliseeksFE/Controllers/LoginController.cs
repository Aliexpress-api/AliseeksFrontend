using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Models.Login;

namespace AliseeksFE.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {

        }

        public IActionResult Login()
        {
            return View(new LoginUserModel());
        }
    }
}
