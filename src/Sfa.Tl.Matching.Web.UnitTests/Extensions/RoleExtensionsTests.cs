using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Extensions
{
    public class RoleExtensionsTests
    {
        [Theory(DisplayName = "Role Extensions GetUserName Data Tests")]
        [InlineData("First", "Last", "First Last")]
        [InlineData("First", null, "First")]
        [InlineData(null, "Last", "Last")]
        [InlineData(null, null, "")]
        public void GetUserNameDataTests(string firstName, string lastName, string expected)
        {
            var claims = new List<Claim>();
            if (firstName != null) claims.Add(new Claim(ClaimTypes.GivenName, firstName));
            if (lastName != null) claims.Add(new Claim(ClaimTypes.Surname, lastName));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var result = claimsPrincipal.GetUserName();

            result.Should().Be(expected);
        }

        [Theory(DisplayName = "Role Extensions GetUserName Data Tests - claims reversed")]
        [InlineData("First", "Last", "First Last")]
        [InlineData("First", null, "First")]
        [InlineData(null, "Last", "Last")]
        [InlineData(null, null, "")]
        public void GetUserNameDataTestsWithClaimsReversed(string firstName, string lastName, string expected)
        {
            var claims = new List<Claim>();
            //Add claims in reversed order
            if (lastName != null) claims.Add(new Claim(ClaimTypes.Surname, lastName));
            if (firstName != null) claims.Add(new Claim(ClaimTypes.GivenName, firstName));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var result = claimsPrincipal.GetUserName();

            result.Should().Be(expected);
        }

        [Theory(DisplayName = "Role Extensions GetUserEmail Data Tests")]
        [InlineData("test@test.com", "test@test.com")]
        [InlineData(null, null)]
        public void GetUserEmailDataTests(string email, string expected)
        {
            var claims = new List<Claim>();
            if (email != null) claims.Add(new Claim(ClaimTypes.Upn, email));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            var result = claimsPrincipal.GetUserEmail();

            result.Should().Be(expected);
        }
    }
}
