using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_No_Search_Results : IClassFixture<OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel>>
    {
        private readonly OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> _fixture;
        private readonly IActionResult _result;
        
        public When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_No_Search_Results(OpportunityControllerFixture<PlacementInformationSaveDto, PlacementInformationSaveViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.OpportunityService.GetSavedOpportunityItemCountAsync(_fixture.OpportunityId).Returns(0);

            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = _fixture.OpportunityId,
                OpportunityItemId = _fixture.OpportunityItemId,
                OpportunityType = OpportunityType.ProvisionGap,
                SearchResultProviderCount = 0,
                JobRole = "Junior Tester",
                PlacementsKnown = false,
                NoSuitableStudent = false,
                HadBadExperience = false,
                ProvidersTooFarAway = false
            };

            _result = _fixture.Sut.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Model_State_Has_No_Errors() =>
            _fixture.Sut.ViewData.ModelState.Count.Should().Be(0);


        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_With_Expected_Field_Values()
        {
            _fixture.OpportunityService
                .Received(3)
                .UpdateOpportunityItemAsync(Arg.Is<PlacementInformationSaveDto>(
                    p => p.OpportunityId == _fixture.OpportunityId &&
                         p.JobRole == "Junior Tester" &&
                         p.PlacementsKnown.HasValue &&
                         !p.PlacementsKnown.Value
            ));
        }

        [Fact]
        public void Then_GetOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService
                .Received(2)
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