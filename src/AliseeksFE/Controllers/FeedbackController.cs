using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Models.Feedback;
using AliseeksFE.Services.Feedback;
using SharpRaven.Core;
using SharpRaven.Core.Data;
using AliseeksFE.Features;
using System.Net;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseeksFE.Controllers
{
    public class FeedbackController : Controller
    {
        IFeedbackService feedback;

        public FeedbackController(IFeedbackService feedback)
        {
            this.feedback = feedback;
        }

        [HttpPost]
        [Route("/feedback")]
        public async Task<IActionResult> Feedback(FeedbackModel model)
        {
            if(ModelState.IsValid)
            {
                var response = await feedback.Submit(model);

                switch(response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        HttpContext.Features.Set<ApplicationMessageFeature>(new ApplicationMessageFeature()
                        {
                            Message = "Feedback has been sent! Thank you for your feedback!",
                            Level = Category.Success
                        });
                        return LocalRedirect("/");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("/feedback")]
        public IActionResult Feedback()
        {
            return View(new FeedbackModel());
        }
    }
}
