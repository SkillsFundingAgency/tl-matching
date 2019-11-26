using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_FindAllProviders_Is_Called_For_Invalid_Postcode
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_FindAllProviders_Is_Called_For_Invalid_Postcode()
        {
            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((false, null));

            var routePathService = Substitute.For<IRoutePathService>();

            var providerProximityService = Substitute.For<IProviderProximityService>();

            var providerProximityController = new ProviderProximityController(routePathService, providerProximityService, locationService);

            const string postcode = "XYZ A12";

            var viewModel = new ProviderProximitySearchParamViewModel
            {
                Postcode = postcode
            };

            _result = providerProximityController.FindAllProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_SearchParametersViewModel()
        {
            _result.Should().NotBeNull();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<ProviderProximitySearchParamViewModel>();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            var viewResult = _result as ViewResult;
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["Postcode"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}