using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace AdminWeb.Controllers
{
    public static class IdentityHelper
    {
        public static int GetAdminId(this IIdentity identity)
        {
            ClaimsIdentity ci = identity as ClaimsIdentity;
            if (ci == null)
                return 0;
            
            var item = ci.FindFirst(ClaimTypes.SerialNumber);
            return item != null ? int.Parse(item.Value) : 0;
        }

        public static String GetValue(IIdentity identity, string field, string defaultValue)
        {
            ClaimsIdentity ci = identity as ClaimsIdentity;
            if (ci == null)
                return defaultValue;

            var item = ci.FindFirst(field);
            return item != null ? item.Value : "";
        }

        public static string GetPlayerName(this IIdentity identity)
        {
            return GetValue(identity, ClaimTypes.Name, "-");
        }
        public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }

        public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
        {
            var identity = currentPrincipal.Identity as ClaimsIdentity;
            if (identity == null)
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim.Value;
        }
    }
}