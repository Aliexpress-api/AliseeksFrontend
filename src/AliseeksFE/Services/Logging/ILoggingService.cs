using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AliseeksFE.Models.Logging;

namespace AliseeksFE.Services.Logging
{
    public interface ILoggingService
    {
        Task<HttpResponseMessage> LogException(LoggingExceptionModel model);
        Task<HttpResponseMessage> LogActivity(LoggingActivityModel model);
    }
}
