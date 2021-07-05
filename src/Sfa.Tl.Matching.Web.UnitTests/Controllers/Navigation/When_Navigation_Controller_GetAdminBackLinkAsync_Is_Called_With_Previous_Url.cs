using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_GetAdminBackLinkAsync_Is_Called_With_Previous_Url
    {
        private string _previousUrl = "https://test.co.uk/target";
        private readonly IActionResult _result;

        public When_Navigation_Controller_GetAdminBackLinkAsync_Is_Called_With_Previous_Url()
        {
            const string userName = "TestUser";

            var opportunityService = Substitute.For<IOpportunityService>();
            var backLinkService = Substitute.For<INavigationService>();
            backLinkService.GetBackLinkAsync(Arg.Any<string>())
                .Returns(_previousUrl);

            var viewModel = new BackLinkViewModel
            {
                ProviderId = 1
            };

            var navigationController = new NavigationController(opportunityService, backLinkService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.GivenName, userName)
                        }))
                    }
                }
            };

            _result = navigationController.GetAdminBackLinkAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Previous_Url()
        {
            var result = _result as RedirectResult;
            result.Should().NotBeNull();

            result?.Url.Should().Be(_previousUrl);
        }
    }
}
