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
    public class When_Placement_Information_Is_Submitted_Successfully_And_There_Is_One_Opportunity : IClassFixture<OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel>>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_Placement_Information_Is_Submitted_Successfully_And_There_Is_One_Opportunity(OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> fixture)
        {
            _fixture = fixture;
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                JobRole = "Junior Tester",
                PlacementsKnown = true,
                Placements = 3
            };


            _fixture.OpportunityService.GetSavedOpportunityItemCountAsync(_fixture.OpportunityId).Returns(0);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_With_Expected_Field_Values()
        {
            _fixture.OpportunityService
                .Received(1)
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
        public void Then_Result_Is_Redirect_To_GetOpportunityCompanyName()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetOpportunityCompanyName");
            result?.RouteValues["opportunityId"].Should().Be(1);
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}