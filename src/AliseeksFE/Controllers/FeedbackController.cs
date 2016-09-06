using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AliseeksFE.Models.Feedback;
using AliseeksFE.Services.Feedback;

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
            var response = await feedback.Submit(model);

            TempData["message"] = "Feedback has been sent! Thank you for your feedback!";
            return LocalRedirect("/");
        }

        [HttpGet]
        [Route("/feedback")]
        public IActionResult Feedback()
        {
            return View(new FeedbackModel());
        }
    }
}
