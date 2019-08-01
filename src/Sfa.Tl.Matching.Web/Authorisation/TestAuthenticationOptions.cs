using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.Authorisation
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, RolesExtensions.AdminUser),
        }, "test");
    }
}