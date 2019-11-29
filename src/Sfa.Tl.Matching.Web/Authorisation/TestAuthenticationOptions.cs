using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.Authorisation
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public static bool IsAdminUser { get; set; }

        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.GivenName, "Dev"),
            new Claim(ClaimTypes.Surname, "Surname"),
            new Claim(ClaimTypes.Upn, "tmatching3@sfa.bis.gov.uk"),
            new Claim(ClaimTypes.Role, IsAdminUser ? RolesExtensions.AdminUser : RolesExtensions.StandardUser)
        }, "test");
    }
}