using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Account
{
    [TestFixture]
    public class Given_I_request_Sign_In
    {
        private AccountController _accountController;

        [SetUp]
        public void Arrange()
        {
            var mockUrlHelper = Substitute.For<IUrlHelper>();

            mockUrlHelper.Action(Arg.Any<UrlActionContext>())
                .Returns("callbackUrl");

            _accountController = new AccountController(Substitute.For<ILogger<AccountController>>())
            {
                Url = mockUrlHelper
            };

        }

        [Test]
        public void Then_I_receive_a_ChallengeResult()
        {
            var result = _accountController.SignIn();

            result.Should().BeOfType<ChallengeResult>();
        }
    }
}