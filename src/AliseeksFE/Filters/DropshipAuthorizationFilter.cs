using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AliseeksFE.Filters
{
    public class DropshipAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var dropshipClaim = context.HttpContext.User.FindFirst(ClaimTypes.Role);

            if(dropshipClaim == null || dropshipClaim.Value == "Expired")
            {
                context.Result = new RedirectToActionResult("dropshipping", "account", null);
                await context.Result.ExecuteResultAsync(context);
            }
        }
    }
}
