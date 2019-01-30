using System.Linq;
using System.Security.Claims;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Infrastructure.Extensions
{
    public static class RolesExtensions
    {
        public static string IdamsUserRole = "http://service/service";
        public static string IdamsUserEmail =      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn";
        public static string IdamsUserGivenName  = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        public static string IdamsUserSurName  =   "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        public const string AdminUser = "TMA";
        public const string StandardUser = "TMS";

        public static bool HasValidRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(StandardUser)
                   || user.IsInRole(AdminUser);
        }

        public static bool HasAdminRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminUser);
        }

        public static bool HasStandardRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(StandardUser);
        }
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNames = user.Claims.Where(c => c.Type == IdamsUserGivenName || c.Type == IdamsUserSurName)
                    .Select(c => c.Value);
            return string.Join(" ", userNames);
        }
        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.Claims.Single(c => c.Type == IdamsUserEmail).Value;
        }
    }
}
