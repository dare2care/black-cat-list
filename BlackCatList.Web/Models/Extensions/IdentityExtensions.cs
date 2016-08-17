namespace Microsoft.AspNet.Identity
{
    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using BlackCatList.Web.Models;

    public static class IdentityExtensions
    {
        public static string GetDisplayName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                return claimsIdentity.FindFirstValue(nameof(User.DisplayName));
            }

            return null;
        }
    }
}