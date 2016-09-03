using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AliseeksFE.Models.Login;
using AliseeksFE.Services.Api;
using System.Security.Claims;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using AliseeksFE.Authentication;

namespace AliseeksFE.Services.User
{
    public class UserService : IUserService
    {
        IApiService api;
        HttpContext context;

        public UserService(IApiService api,
            IHttpContextAccessor accessor)
        {
            this.api = api;
            context = accessor.HttpContext;
        }

        public async Task<HttpResponseMessage> Login(LoginUserModel model)
        {
            HttpResponseMessage mess = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var token = new AliseeksJwtAuthentication().GenerateToken(claims);
            context.Response.Cookies.Append("access_token", token,
                new CookieOptions()
                {
                    Path = "/",
                    Domain = "",
                    HttpOnly = false,
                    Secure = false,
                    Expires = DateTimeOffset.Now.AddDays(14)
                });

            return mess;
        }

        public async Task<HttpResponseMessage> Register(NewUserModel model)
        {
            return null;
        }

        public async Task<HttpResponseMessage> Reset(ResetUserModel model)
        {
            return null;
        }
    }
}
