using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TrueFalse.Auth.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        internal const string UserIdClaims = "userId";

        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == UserIdClaims);
            if (claim == null)
            {
                throw new NullReferenceException("В claimsPrincipal не установлен userId claim");
            }

            return new Guid(claim.Value);
        }
    }
}
