using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Loaded_With_Placements_Unknown
    {
        private readonly IActionResult _result;

        private readonly PlacementInformationSaveDto _dto = new PlacementInformationSaveDto();
        private const bool PlacementsKnown = false;
        private const int Placements = 5;
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 12;

        public When_Placement_Information_Loaded_With_Placements_Unknown()
        {
            _dto.OpportunityId = OpportunityId;
            _dto.PlacementsKnown = PlacementsKnown;
            _dto.Placements = Placements;

            var config = new MapperConfiguration(c => c.AddMaps(typeof(PlacementInformationSaveDtoMapper).Assembly));
            var mapper = new Mapper(config);
            
            var opportunityService = Substitute.For<IOpportunityService>();
            opportunityService.GetPlacementInformationAsync(Arg.Any<int>()).Returns(_dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(opportunityService, referralService, mapper);

            _result = opportunityController.GetPlacementInformation(OpportunityItemId).GetAwaiter().GetResult();
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