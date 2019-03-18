using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Loaded_With_Placements_Unknown
    {
        private readonly IActionResult _result;

        private readonly OpportunityDto _dto = new OpportunityDto();
        private const bool PlacementsKnown = false;
        private const int Placements = 5;
        private const int OpportunityId = 12;

        public When_Placement_Information_Loaded_With_Placements_Unknown()
        {
            _dto.Id = OpportunityId;
            _dto.PlacementsKnown = PlacementsKnown;
            _dto.Placements = Placements;

            var opportunityService = Substitute.For<IOpportunityService>();
            opportunityService.GetOpportunity(Arg.Any<int>()).Returns(_dto);

            var opportunityController = new OpportunityController(opportunityService);

            _result = opportunityController.PlacementInformationSave(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_PlacementsKnown_Is_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.PlacementsKnown.Should().Be(PlacementsKnown);
        }

        [Fact]
        public void Then_Placements_Is_Set_To_Default_Int()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.Placements.Should().BeNull();
        }
    }
}