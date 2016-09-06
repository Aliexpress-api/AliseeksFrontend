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
            if(ModelState.IsValid)
            {
                user.Login(model);
                return LocalRedirect("/");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Route("/logout")]
        public IActionResult Logout()
        {
            user.Logout();
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
            if(model.Password != model.ConfirmPassword)
                ModelState.AddModelError("Password", "Password and confirm password must match");

            if(ModelState.IsValid)
            {
                user.Register(model);
                return LocalRedirect("/login");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Route("/passwordreset")]
        public IActionResult PasswordReset()
        {
            return View(new ResetUserModel());
        }

        [HttpPost]
        [Route("/passwordreset")]
        public IActionResult PasswordReset(ResetUserModel model)
        {
            if(ModelState.IsValid)
            {
                user.Reset(model);

                TempData["message"] = "New password has been sent to your mailbox";
                return LocalRedirect("/");
            }
            else
            {
                return View(model);
            }
        }
    }
}
