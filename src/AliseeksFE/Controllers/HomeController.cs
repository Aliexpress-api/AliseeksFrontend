using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using AliseeksFE.Services.Api;

using AliseeksFE.Models.Login;
using AliseeksFE.Services.Logging;
using AliseeksFE.Models.Logging;
using Microsoft.Extensions.Logging;

namespace AliseeksFE.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Error")]
        public async Task<IActionResult> Error([FromServices]ILoggingService logging)
        {
            try
            {
                var error = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
                var path = HttpContext.Features.Get<AliseeksFE.Middleware.LoggerFeature>().Path;

                var model = new LoggingExceptionModel()
                {
                    Criticality = 5,
                    Message = $"Query: {path}\n{error.Message}",
                    StackTrace = error.StackTrace
                };

                await logging.LogException(model);
            }
            catch(Exception e)
            {
                logger.LogError(new EventId(501), e, "ERROR IN ERROR LOGGER");
            }

            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/feedback")]
        public IActionResult Feedback()
        {
            return View();
        }
    }
}
