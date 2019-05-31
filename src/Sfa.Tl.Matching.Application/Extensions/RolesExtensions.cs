// ReSharper disable once RedundantUsingDirective
using System.Linq;
using System.Security.Claims;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class RolesExtensions
    {
        public static string IdamsUserRole = "http://service/service";
        public const string AdminUser = "TMA";
        public const string StandardUser = "TMS";

        public static bool HasValidRole(this ClaimsPrincipal user)
        {
#if NOAUTH
            return true;
#else
            return user.IsInRole(StandardUser)
                   || user.IsInRole(AdminUser);
#endif        
        }

        public static bool HasAdminRole(this ClaimsPrincipal user)
        {
#if NOAUTH
            return true;
#else
            return user.IsInRole(AdminUser);
#endif        
        }

        public static bool IsAuthorisedAdminUser(this ClaimsPrincipal user, string authorisedUserEmail)
        {
#if NOAUTH
            return true;
#else
            return HasAdminRole(user)
                && GetUserEmail(user) == authorisedUserEmail;
#endif        
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
#if NOAUTH
            return "No Auth Admin";
#else
            var userNames = user.Claims.Where(c => c.Type == ClaimTypes.GivenName || c.Type == ClaimTypes.Surname)
                    .Select(c => c.Value);
            return string.Join(" ", userNames);
#endif
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
#if NOAUTH
            return "No.Auth@Admin.com";
#else
            return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Upn)?.Value;
#endif
        }
    }
}
