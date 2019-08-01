using System;
using Microsoft.AspNetCore.Authentication;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Local Scheme", "Local Auth", configureOptions);
        }
    }
}