using System.Security.Claims;

namespace Sfa.Tl.Matching.Web.Extensions
{
    public static class Roles
    {
        public const string RoleClaimType = "http://service/service";

        public const string AdminUser = "TMA";
        public const string StandardUser = "TMS";

        public static bool HasValidRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(StandardUser)
                   || user.IsInRole(AdminUser);

        }
    }
}
