using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndAddVenue
    {
        private readonly IActionResult _result;
        private readonly IProviderService _providerService;

        public When_Provider_Controller_SaveProviderDetail_Is_Called_With_SubmitAction_SaveAndAddVenue()
        {
            _providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(_providerService, new MatchingConfiguration());

            _result = providerController.SaveProviderDetailAsync(new ProviderDetailViewModel
            {
                Id = 1,
                SubmitAction = "SaveAndAddVenue"
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_AddVenue()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("AddVenue");
        }

        [Fact]
        public void Then_ProviderService_UpdateProviderDetail_Called()
        {
            _providerService.Received(1).UpdateProviderDetailAsync(Arg.Any<ProviderDetailViewModel>());
        }
    }
}