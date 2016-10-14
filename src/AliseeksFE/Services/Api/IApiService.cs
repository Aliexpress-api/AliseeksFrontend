using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace AliseeksFE.Services.Api
{
    public interface IApiService
    {
        Task<HttpResponseMessage> Get(string endpoint, Action<HttpClient> clientConfig = null);
        Task<HttpResponseMessage> Post(string endpoint, StringContent data, Action<HttpClient> clientConfig = null);
        Task<HttpResponseMessage> Delete(string endpoint, Action<HttpClient> clientConfig = null);
        Task<HttpResponseMessage> Put(string endpoint, StringContent data, Action<HttpClient> clientConfig = null);
    }
}
