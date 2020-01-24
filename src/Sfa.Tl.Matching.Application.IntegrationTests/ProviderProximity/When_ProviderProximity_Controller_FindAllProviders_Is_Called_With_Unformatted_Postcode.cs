using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderProximity
{
    public class When_ProviderProximity_Controller_FindAllProviders_Is_Called_With_Unformatted_Postcode : IClassFixture<ProviderProximityControllerFixture>
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_FindAllProviders_Is_Called_With_Unformatted_Postcode(ProviderProximityControllerFixture fixture)
        {
            const string requestPostcode = "cV12 Wt";
            
            fixture.GetProviderProximityController(requestPostcode);
            
            _result = fixture.ProviderProximityController.FindAllProviders(new ProviderProximitySearchParamViewModel
            {
                Postcode = requestPostcode
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderProximityResults");

            redirect?.RouteValues["searchCriteria"].Should().Be("CV1 2WT");
        }
    }
}