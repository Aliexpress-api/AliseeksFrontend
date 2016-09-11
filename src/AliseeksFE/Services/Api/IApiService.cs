using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace AliseeksFE.Services.Api
{
    public interface IApiService
    {
        Task<HttpResponseMessage> Get(string endpoint);
        Task<HttpResponseMessage> Post(string endpoint, StringContent data);
        Task<HttpResponseMessage> AnonymousPost(string endpoint, StringContent data);
    }
}
