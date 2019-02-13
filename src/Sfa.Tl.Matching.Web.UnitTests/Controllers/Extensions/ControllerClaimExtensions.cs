using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using System.Security.Claims;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions
{
    public static class ControllerClaimExtensions
    {
        public static void AddStandardUserToContext(this Controller controller)
        {
            AddClaim(controller, ClaimTypes.Role, RolesExtensions.StandardUser);
        }

        public static void AddUsernameToContext(this Controller controller, string username)
        {
            AddClaim(controller, ClaimTypes.GivenName, username);
        }

        private static void AddClaim(Controller controller, string type, string value)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(type, value),
            }));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}