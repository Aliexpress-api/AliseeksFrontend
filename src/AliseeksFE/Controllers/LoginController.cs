using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Models.Login;
using AliseeksFE.Services.User;
using System.Net;
using AliseeksFE.Utility.Extensions;
using AliseeksFE.Features;

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
        public IActionResult Login(string returnUrl)
        {
            if(!returnUrl.IsEmptyOrNull())
                ViewData.Add("ReturnUrl", returnUrl);

            return View(new LoginUserModel());
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginUserModel model, string returnUrl)
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
                        if (returnUrl.IsEmptyOrNull())
                            return LocalRedirect("/");
                        else
                            return LocalRedirect(returnUrl);

                    default:
                        return View(model);
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
        public IActionResult Register(string returnUrl)
        {
            if (!returnUrl.IsEmptyOrNull())
                ViewData.Add("ReturnUrl", returnUrl);

            return View(new NewUserModel());
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register(NewUserModel model, string returnUrl)
        {
            object routeValues = null;

            if (!returnUrl.IsEmptyOrNull())
                routeValues = new { returnUrl = returnUrl };

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
                        return RedirectToAction("Login", routeValues);
                }
            }
            else
            {
                if (!returnUrl.IsEmptyOrNull())
                    ViewData.Add("ReturnUrl", returnUrl);

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
                var response = await user.Reset(model);

                switch(response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        HttpContext.Features.Set<ApplicationMessageFeature>(new ApplicationMessageFeature()
                        {
                            Message = "A password reset token has been sent to your mailbox",
                            Level = Category.Success
                        });
                        return LocalRedirect("/");

                    default:
                        return View(model);
                }
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
