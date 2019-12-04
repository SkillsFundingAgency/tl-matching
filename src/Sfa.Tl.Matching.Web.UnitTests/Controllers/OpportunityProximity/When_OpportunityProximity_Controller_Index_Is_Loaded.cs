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
    public class When_OpportunityProximity_Controller_Index_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_OpportunityProximity_Controller_Index_Is_Loaded()
        {
            var proximityopportunityService = Substitute.For<IOpportunityProximityService>();
            var locationService = Substitute.For<ILocationService>();
            var routeService = Substitute.For<IRoutePathService>();

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(routeService, proximityopportunityService, _opportunityService, locationService);

            _result = opportunityProximityController.Index().GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_OpportunityService_GetCompanyNameWithAkaAsync_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceiveWithAnyArgs()
                .GetCompanyNameWithAkaAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ViewModel_CompanyNameWithAka_Should_Not_Be_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<SearchParametersViewModel>();
            viewModel.CompanyNameWithAka.Should().BeNull();
        }
    }
}