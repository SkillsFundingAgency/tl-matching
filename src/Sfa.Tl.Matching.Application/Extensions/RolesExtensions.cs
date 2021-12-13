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
            return user.IsInRole(StandardUser)
                   || user.IsInRole(AdminUser);
        }

        public static bool HasAdminRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminUser);
        }

        public static bool IsAuthorisedAdminUser(this ClaimsPrincipal user, string authorisedUserEmail)
        {
            return HasAdminRole(user)
                && GetUserEmail(user) == authorisedUserEmail;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            var firstName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var surname = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            return $"{firstName}{(firstName is not null && surname is not null ? " " : null)}{surname}";
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Upn)?.Value;
        }
    }
}