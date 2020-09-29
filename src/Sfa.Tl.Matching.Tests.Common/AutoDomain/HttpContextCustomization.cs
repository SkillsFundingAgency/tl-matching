using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Http;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class HttpContextCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<HttpContext>(() => new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });
        }
    }
}