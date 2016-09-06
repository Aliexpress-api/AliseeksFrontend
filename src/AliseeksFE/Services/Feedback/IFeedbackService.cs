using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AliseeksFE.Models.Feedback;

namespace AliseeksFE.Services.Feedback
{
    public interface IFeedbackService
    {
        Task<HttpResponseMessage> Submit(FeedbackModel model);
    }
}
