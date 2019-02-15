using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
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
            _accountController.AddUsernameToContext("username");

            var result = _accountController.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult?.ControllerName.Should().Be("Home");
            redirectResult?.ActionName.Should().Be("InvalidRole");
        }

        [Fact]
        public void And_I_do_not_have_correct_role_Then_redirect_to_search_start_page()
        {
            _accountController.AddStandardUserToContext();

            var result = _accountController.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult?.ControllerName.Should().Be("Search");
            redirectResult?.ActionName.Should().Be("Start");
        }
    }
}