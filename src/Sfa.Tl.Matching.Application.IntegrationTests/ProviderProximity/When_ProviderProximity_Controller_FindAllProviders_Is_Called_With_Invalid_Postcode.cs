using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderProximity
{
    public class When_ProviderProximity_Controller_FindAllProviders_Is_Called_With_Invalid_Postcode : IClassFixture<ProviderProximityControllerFixture>
    {
        private readonly ProviderProximityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_FindAllProviders_Is_Called_With_Invalid_Postcode(ProviderProximityControllerFixture fixture)
        {
            _fixture = fixture;
            
            const string requestPostcode = "cV12 34";

            _fixture.GetProviderProximityController(requestPostcode);
            
            _result = _fixture.ProviderProximityController.FindAllProviders(new ProviderProximitySearchParamViewModel
            {
                Postcode = requestPostcode
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            _fixture.ProviderProximityController.ViewData.ModelState.IsValid.Should().BeFalse();
            _fixture.ProviderProximityController.ViewData.ModelState["Postcode"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}