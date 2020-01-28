using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_CreateProviderDetail_Is_Called_With_SubmitAction_SaveAndAddVenue : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;

        public When_Provider_Controller_CreateProviderDetail_Is_Called_With_SubmitAction_SaveAndAddVenue()
        {
            _fixture = new ProviderControllerFixture();

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.ProviderService.CreateProviderAsync(Arg.Any<CreateProviderDetailViewModel>())
                .Returns(1);

            _result = controllerWithClaims.CreateProviderDetailAsync(new CreateProviderDetailViewModel
            {
                SubmitAction = "SaveAndAddVenue"
            }).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Result_Is_Redirect_To_AddVenue()
        {
            _result.Should().NotBeNull();
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("AddProviderVenue");
            result?.RouteValues["providerId"].Should().Be(1);
        }

        [Fact]
        public void Then_ProviderService_CreateProvider_Called()
        {
            _fixture.ProviderService.Received(1).CreateProviderAsync(Arg.Any<CreateProviderDetailViewModel>());
        }
    }
}