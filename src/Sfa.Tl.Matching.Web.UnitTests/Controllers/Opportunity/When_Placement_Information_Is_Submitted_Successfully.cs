using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private readonly PlacementInformationViewModel _viewModel = new PlacementInformationViewModel();
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Placement_Information_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("username")
                .Build();

            _result = controllerWithClaims.Placements(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).SavePlacementInformation(_viewModel);
        }

        [Fact]
        public void Then_Result_Is_Redirect_to_FindEmployer()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("EmployerFind_Get");
        }
    }
}