using System.Security.Claims;

namespace Sfa.Tl.Matching.Web.Extensions
{
    public static class Roles
    {
        public const string RoleClaimType = "http://service/service";

        public const string AdminUser = "EPC";
        public const string StandardUser = "EPO";

        public static bool HasValidRole(this ClaimsPrincipal user)
        {
            return user.IsInRole(StandardUser)
                   || user.IsInRole(AdminUser);

        }
    }
}
