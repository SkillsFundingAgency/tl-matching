using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_Search_Results : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Placement_Information_Is_Submitted_For_Provision_Gap_With_No_Reason_And_There_Are_Search_Results(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            var viewModel = new PlacementInformationSaveViewModel
            {
                OpportunityId = _fixture.OpportunityId,
                OpportunityItemId = _fixture.OpportunityItemId,
                OpportunityType = OpportunityType.ProvisionGap,
                SearchResultProviderCount = 1,
                NoSuitableStudent = false,
                HadBadExperience = false,
                ProvidersTooFarAway = false
            };

            _result = _fixture.Sut.SavePlacementInformationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
        }

        [Fact]
        public void Then_Model_State_Has_No_Reason_Given_Error()
        {
            _fixture.Sut.ViewData.ModelState.Should().ContainSingle();

            _fixture.Sut.ViewData.ModelState.ContainsKey(nameof(PlacementInformationSaveViewModel.NoSuitableStudent))
                .Should().BeTrue();

            var modelStateEntry = _fixture.Sut.ViewData.ModelState[nameof(PlacementInformationSaveViewModel.NoSuitableStudent)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must tell us why the employer did not choose a provider");
        }
    }
}