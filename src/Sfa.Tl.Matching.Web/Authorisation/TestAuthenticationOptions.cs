using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.Authorisation
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.GivenName, "Dev"),
            new Claim(ClaimTypes.Surname, "Surname"),
            new Claim(ClaimTypes.Upn, "dev@email.com"),
            new Claim(ClaimTypes.Role, RolesExtensions.AdminUser)
        }, "test");
    }
}