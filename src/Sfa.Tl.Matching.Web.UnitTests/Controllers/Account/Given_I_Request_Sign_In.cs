using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Account
{
    public class Given_I_Request_Sign_In : IClassFixture<AccountControllerFixture>
    {
        private readonly AccountControllerFixture _fixture;

        public Given_I_Request_Sign_In(AccountControllerFixture fixture)
        {
            _fixture = fixture;

            _fixture.MockUrlHelper.Action(Arg.Any<UrlActionContext>())
                .Returns("callbackUrl");
        }

        [Fact]
        public void Then_I_Receive_A_ChallengeResult()
        {
            var result = _fixture.AccountController.SignIn();

            result.Should().BeOfType<ChallengeResult>();
        }
    }
}