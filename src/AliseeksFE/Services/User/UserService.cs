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
using AliseeksFE.Models.Account;

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
                Domain = context.Request.Host.Host,
                Expires = DateTimeOffset.Now.AddDays(-1)
            });
        }

        public async Task<HttpResponseMessage> Login(LoginUserModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await api.Post(ApiEndpoints.UserLogin, content);
            
            //If login is unsuccessful do not set cookie
            if (!response.IsSuccessStatusCode)
                return response;            

            return response;
        }

        public async Task<HttpResponseMessage> Register(NewUserModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await api.Post(ApiEndpoints.UserRegister, content);

            return response;
        }

        public async Task<HttpResponseMessage> Reset(ResetUserModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await api.Post(ApiEndpoints.UserReset, content);
            return response;
        }

        public async Task<HttpResponseMessage> ResetValid(ResetValidModel model)
        {
            var content = new StringContent(JsonConvert.SerializeObject(model));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await api.Post(ApiEndpoints.UserResetValid, content);
            return response;
        }

        public async Task<UserOverview> Overview(string username)
        {
            var endpoint = ApiEndpoints.UserAccount(username);
            var response = await api.Get(endpoint);

            var json = await response.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<UserOverview>(await response.Content.ReadAsStringAsync());

            return model;
        }
    }
}
