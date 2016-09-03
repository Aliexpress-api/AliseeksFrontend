using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Models.Login;
using AliseeksFE.Services.User;

namespace AliseeksFE.Controllers
{
    public class LoginController : Controller
    {
        IUserService user;

        public LoginController(IUserService service)
        {
            user = service;
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            return View(new LoginUserModel());
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(LoginUserModel model)
        {
            user.Login(model);
            return LocalRedirect("/");
        }

        [HttpGet]
        [Route("/register")]
        public IActionResult Register()
        {
            return View(new NewUserModel());
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(NewUserModel model)
        {
            return View();
        }
    }
}
