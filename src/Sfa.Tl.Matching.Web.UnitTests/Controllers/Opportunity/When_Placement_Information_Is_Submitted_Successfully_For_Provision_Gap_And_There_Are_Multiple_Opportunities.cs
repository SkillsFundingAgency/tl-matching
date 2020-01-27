using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Successfully_For_Provision_Gap_And_There_Are_Multiple_Opportunities : IClassFixture<OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel>>
    {
        private readonly OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Placement_Information_Is_Submitted_Successfully_For_Provision_Gap_And_There_Are_Multiple_Opportunities(OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> fixture)
        {
            _fixture = fixture;
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                OpportunityType = OpportunityType.ProvisionGap,
                JobRole = "Junior Tester",
                PlacementsKnown = true,
                Placements = 3,
                NoSuitableStudent = true
            };

            _fixture.OpportunityService.GetSavedOpportunityItemCountAsync(_fixture.OpportunityId).Returns(2);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_With_Expected_Field_Values()
        {
            _fixture.OpportunityService
                .Received(2)
                .UpdateOpportunityItemAsync(Arg.Is<PlacementInformationSaveDto>(
                p => p.OpportunityId == _fixture.OpportunityId &&
                    p.JobRole == "Junior Tester" &&
                    p.PlacementsKnown.HasValue &&
                    p.PlacementsKnown.Value &&
                    p.Placements == 3
            ));
        }

        [Fact]
        public void Then_GetOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService
                .Received(3)
                .GetSavedOpportunityItemCountAsync(_fixture.OpportunityId);
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetOpportunityBasket");
            result?.RouteValues["opportunityId"].Should().Be(_fixture.OpportunityId);
            result?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }
    }
}