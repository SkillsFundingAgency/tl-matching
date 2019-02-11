using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;

using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Account
{

    public class Given_I_Request_Sign_In
    {
        private AccountController _accountController;

        
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

        [Fact]
        public void Then_I_receive_a_ChallengeResult()
        {
            var result = _accountController.SignIn();

            result.Should().BeOfType<ChallengeResult>();
        }
    }
}