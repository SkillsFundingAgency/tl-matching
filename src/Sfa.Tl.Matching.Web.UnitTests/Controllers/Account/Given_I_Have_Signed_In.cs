using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
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
        public void And_I_Do_Not_Have_Correct_Role_Then_Redirect_To_FailedLogin_Page()
        {
            var controllerWithClaims = new ClaimsBuilder<AccountController>(_accountController)
                .AddUserName("username")
                .Build();

            var result = controllerWithClaims.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult?.ControllerName.Should().Be("Home");
            redirectResult?.ActionName.Should().Be("FailedLogin");
        }

        [Fact]
        public void And_I_Do_Not_Have_Correct_Role_Then_Redirect_To_Search_Start_Page()
        {
            var controllerWithClaims = new ClaimsBuilder<AccountController>(_accountController)
                .AddStandardUser()
                .Build();

            var result = controllerWithClaims.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull(); 
            redirectResult?.ControllerName.Should().Be("OpportunityProximity");
            redirectResult?.ActionName.Should().Be("Start");
        }
    }
}