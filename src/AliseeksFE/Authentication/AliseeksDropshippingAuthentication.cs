using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AliseeksFE.Services.Dropshipping;

namespace AliseeksFE.Authentication
{
    public class AliseeksDropshippingAuthentication : IAuthorizationService
    {
        private readonly DropshipService dropship;

        public AliseeksDropshippingAuthentication(DropshipService dropshipping)
        {
            dropship = dropshipping;
        }

        public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            var account = await dropship.GetAccount();

            if (account.Status == Models.Dropship.AccountStatus.New)
                return false;

            return true;
        }

        public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            var account = await dropship.GetAccount();

            if (account.Status == Models.Dropship.AccountStatus.New)
                return false;

            return true;
        }
    }
}
