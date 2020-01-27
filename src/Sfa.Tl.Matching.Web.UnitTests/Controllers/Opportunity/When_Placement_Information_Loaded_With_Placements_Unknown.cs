using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Loaded_With_Placements_Unknown : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        private readonly PlacementInformationSaveDto _dto = new PlacementInformationSaveDto();
        private const bool PlacementsKnown = false;
        private const int Placements = 5;
        
        public When_Placement_Information_Loaded_With_Placements_Unknown(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            _dto.OpportunityId = _fixture.OpportunityId;
            _dto.PlacementsKnown = PlacementsKnown;
            _dto.Placements = Placements;

            _fixture.OpportunityService.GetPlacementInformationAsync(Arg.Any<int>()).Returns(_dto);

            _result = _fixture.Sut.GetPlacementInformationAsync(_fixture.OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ViewModel_Fields_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.PlacementsKnown.Should().Be(PlacementsKnown);
        
            viewModel.Placements.Should().BeNull();
        }
    }
}