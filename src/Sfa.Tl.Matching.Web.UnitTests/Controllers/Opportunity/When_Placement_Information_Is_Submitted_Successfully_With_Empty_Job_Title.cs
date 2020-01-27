using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Successfully_With_Empty_Job_Title : IClassFixture<OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel>>
    {
        private readonly OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Placement_Information_Is_Submitted_Successfully_With_Empty_Job_Title(OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> fixture)
        {
            _fixture = fixture;
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = _fixture.OpportunityId,
                OpportunityItemId = _fixture.OpportunityItemId,
                JobRole = null,
                PlacementsKnown = false
            };

            _fixture.OpportunityService.GetSavedOpportunityItemCountAsync(_fixture.OpportunityId).Returns(0);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_With_Default_Job_Title()
        {
            _fixture.OpportunityService
                .Received(3)
                .UpdateOpportunityItemAsync(Arg.Is<PlacementInformationSaveDto>(
                p => p.OpportunityId == _fixture.OpportunityId &&
                     string.IsNullOrEmpty(p.JobRole)));
        }

        [Fact]
        public void Then_GetOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService
                .Received(1)
                .GetSavedOpportunityItemCountAsync(_fixture.OpportunityId);
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityCompanyName()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetOpportunityCompanyName");
            result?.RouteValues["opportunityId"].Should().Be(_fixture.OpportunityId);
            result?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }
    }
}