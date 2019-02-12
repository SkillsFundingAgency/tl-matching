using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Account
{
    public class Given_I_Have_Signed_In
    {
        private readonly AccountController _accountController;

        public Given_I_Have_Signed_In()
        {
            _accountController = new AccountController(Substitute.For<ILogger<AccountController>>());

        }

        [Fact]
        public void And_I_do_not_have_correct_role_Then_redirect_to_InvalidRole_page()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.GivenName, "username"),
            }));

            _accountController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _accountController.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult?.ControllerName.Should().Be("Home");
            redirectResult?.ActionName.Should().Be("InvalidRole");
        }

        [Fact]
        public void And_I_do_not_have_correct_role_Then_redirect_to_search_start_page()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Role, RolesExtensions.StandardUser),
            }));

            _accountController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _accountController.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult?.ControllerName.Should().Be("Search");
            redirectResult?.ActionName.Should().Be("Start");
        }
    }
}