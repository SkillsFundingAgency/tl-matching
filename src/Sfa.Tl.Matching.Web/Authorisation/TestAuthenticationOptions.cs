using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
        {
            new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", RolesExtensions.AdminUser),
        }, "test");
    }
}