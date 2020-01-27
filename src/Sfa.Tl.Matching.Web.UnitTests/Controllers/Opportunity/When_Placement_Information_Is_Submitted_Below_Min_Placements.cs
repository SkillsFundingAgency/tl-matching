using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Below_Min_Placements : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Placement_Information_Is_Submitted_Below_Min_Placements(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            

            var viewModel = new PlacementInformationSaveViewModel
            {
                PlacementsKnown = true,
                Placements = 0
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
        public void hen_Model_State_Has_Min_Error()
        {
            _fixture.Sut.ViewData.ModelState.Should().ContainSingle();

            _fixture.Sut.ViewData.ModelState.ContainsKey(nameof(PlacementInformationSaveViewModel.Placements))
                .Should().BeTrue();

            var modelStateEntry = _fixture.Sut.ViewData.ModelState[nameof(PlacementInformationSaveViewModel.Placements)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("The number of students must be 1 or more");
        }
    }
}