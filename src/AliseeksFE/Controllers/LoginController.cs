using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Models.Login;
using AliseeksFE.Services.User;
using System.Net;

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
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if(ModelState.IsValid)
            {
                var response = await user.Login(model);
                switch(response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        ModelState.AddModelError("All", "Username and password did not match");
                        return View(model);

                    case HttpStatusCode.OK:
                    default:
                        return LocalRedirect("/");
                }
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
        public async Task<IActionResult> Register(NewUserModel model)
        {
            if(ModelState.IsValid)
            {
                var response = await user.Register(model);
                switch(response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        ModelState.AddModelError("All", await response.Content.ReadAsStringAsync());
                        return View(model);

                    case HttpStatusCode.OK:
                    default:
                        return LocalRedirect("/login");
                }
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
        public async Task<IActionResult> PasswordReset(ResetUserModel model)
        {
            if(ModelState.IsValid)
            {
                await user.Reset(model);

                TempData["message"] = "New password has been sent to your mailbox";
                return LocalRedirect("/");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Route("/user/reset")]
        public IActionResult PasswordResetValid(string token)
        {
            return View(new ResetValidModel() { Token = token });
        }

        [HttpPost]
        [Route("/user/reset")]
        public async Task<IActionResult> PasswordResetValid(ResetValidModel model)
        {
            if (model.NewPassword != model.ConfirmNewPassword)
                ModelState.AddModelError("Password", "Password and confirm password must match");

            if (ModelState.IsValid)
            {
                var response = await user.ResetValid(model);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        ModelState.AddModelError("All", await response.Content.ReadAsStringAsync());
                        return View(model);

                    case HttpStatusCode.OK:
                    default:
                        return LocalRedirect("/login");
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
