using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Account
{
    [TestFixture]
    public class Given_I_have_signed_in
    {
        private IHttpContextAccessor _contextAccessor;
        private AccountController _accountController;
        
        [SetUp]
        public void Arrange()
        {
            _accountController = new AccountController(Substitute.For<ILogger<AccountController>>());
        }

        [Test]
        public void And_I_do_not_have_correct_role_Then_redirect_to_InvalidRole_page()
        {
            _contextAccessor = Substitute.For<IHttpContextAccessor>();

            _contextAccessor.HttpContext.User.IsInRole(Arg.Any<string>())
                .Returns(false);

            var result = _accountController.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult?.ControllerName.Should().Be("Home");
            redirectResult?.ActionName.Should().Be("InvalidRole");
        }

        [Test]
        public void And_I_do_not_have_correct_role_Then_redirect_to_search_start_page()
        {
            _contextAccessor = Substitute.For<IHttpContextAccessor>();

            _contextAccessor.HttpContext.User.IsInRole(Arg.Any<string>())
                .Returns(true);

            var result = _accountController.PostSignIn();

            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult?.ControllerName.Should().Be("Search");
            redirectResult?.ActionName.Should().Be("Start");
        }
    }
}