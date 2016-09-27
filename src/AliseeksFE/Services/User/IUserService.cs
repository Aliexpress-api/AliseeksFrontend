using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AliseeksFE.Models.Login;
using AliseeksFE.Models.Account;

namespace AliseeksFE.Services.User
{
    public interface IUserService
    {
        void Logout();
        Task<HttpResponseMessage> Login(LoginUserModel model);
        Task<HttpResponseMessage> Register(NewUserModel model);
        Task<HttpResponseMessage> Reset(ResetUserModel model);
        Task<HttpResponseMessage> ResetValid(ResetValidModel model);
        Task<UserOverview> Overview(string username);
    }
}
