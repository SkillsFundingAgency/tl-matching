using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndAddVenue : IClassFixture<ProviderControllerFixture>
    {
        private readonly IActionResult _result;
        private readonly ProviderControllerFixture _fixture;
        

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndAddVenue()
        {
            _fixture = new ProviderControllerFixture();
            
            _result = _fixture.Sut.SaveProviderDetailAsync(new ProviderDetailViewModel
            {
                Id = 1,
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
            result?.RouteValues
                .Should()
                .Contain(new KeyValuePair<string, object>("providerId", 1));
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetail_Called()
        {
            _fixture.ProviderService.Received(1).UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
        }
    }
}