using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Account
{
    public class Given_I_Have_Signed_In : IClassFixture<AccountControllerFixture>
    {
        private readonly AccountControllerFixture _fixture;
        
        public Given_I_Have_Signed_In(AccountControllerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void And_I_Do_Not_Have_Correct_Role_Then_Redirect_To_FailedLogin_Page()
        {
            //Arrange
            var controllerWithClaims = _fixture.GetFailedLoginUser;

            //Act
            var result = controllerWithClaims.PostSignIn();

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull();
            redirectResult?.ControllerName.Should().Be("Home");
            redirectResult?.ActionName.Should().Be("FailedLogin");
        }

        [Fact]
        public void And_I_Do_Not_Have_Correct_Role_Then_Redirect_To_Search_Start_Page()
        {
            //Arrange
            var controllerWithClaims = _fixture.GetStandardLoginUser;

            //Act
            var result = controllerWithClaims.PostSignIn();

            //Assert
            result.Should().BeOfType<RedirectToActionResult>();

            var redirectResult = result as RedirectToActionResult;
            redirectResult.Should().NotBeNull(); 
            redirectResult?.ControllerName.Should().Be("Dashboard");
            redirectResult?.ActionName.Should().Be("Start");
        }
    }
}
