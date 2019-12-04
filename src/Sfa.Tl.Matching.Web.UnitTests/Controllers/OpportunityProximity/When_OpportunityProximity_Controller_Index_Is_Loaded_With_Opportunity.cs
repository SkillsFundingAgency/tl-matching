using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_Index_Is_Loaded_With_Opportunity
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_OpportunityProximity_Controller_Index_Is_Loaded_With_Opportunity()
        {
            var locationService = Substitute.For<ILocationService>();
            var opportunityProximityService = Substitute.For<IOpportunityProximityService>();
            var routeService = Substitute.For<IRoutePathService>();

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService
                .GetCompanyNameWithAkaAsync(1)
                .Returns("CompanyName (AlsoKnownAs)");

            var opportunityProximityController = new OpportunityProximityController(routeService, opportunityProximityService, _opportunityService, locationService);

            _result = opportunityProximityController.Index(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityService_GetCompanyNameWithAkaAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetCompanyNameWithAkaAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ViewModel_CompanyNameWithAka_Should_Have_Expected_Value()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<SearchParametersViewModel>();
            viewModel.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");
        }
    }
}