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
using Microsoft.Extensions.Options;
using AliseeksFE.Configuration.Options;
using Newtonsoft.Json;

namespace AliseeksFE.Services.User
{
    public class UserService : IUserService
    {
        IApiService api;
        HttpContext context;
        AliseeksJwtAuthentication auth;

        public UserService(IApiService api,
            IHttpContextAccessor accessor, AliseeksJwtAuthentication auth)
        {
            this.auth = auth;
            this.api = api;
            context = accessor.HttpContext;
        }

        public void Logout()
        {
            if (!context.Request.Cookies.ContainsKey("access_token"))
                return;

            context.Response.Cookies.Delete("access_token", new CookieOptions
            {
                Path = "/",
                Domain = "",
                Expires = DateTimeOffset.Now.AddDays(-1)
            });
        }

        public async Task<HttpResponseMessage> Login(LoginUserModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await api.Post(ApiEndpoints.UserLogin, content);

            var token = JsonConvert.DeserializeObject<ResponseLoginUserModel>(await response.Content.ReadAsStringAsync());

            context.Response.Cookies.Append("access_token", token.Token,
                new CookieOptions()
                {
                    Path = "/",
                    Domain = "",
                    HttpOnly = false,
                    Secure = false,
                    Expires = DateTimeOffset.Now.AddDays(14)
                });

            return response;
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
