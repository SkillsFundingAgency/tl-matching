using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Placement_Information_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private readonly OpportunityDto _dto = new OpportunityDto();
        private const string JobTitle = "JobTitle";
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const string ModifiedBy = "ModifiedBy";
        private readonly PlacementInformationViewModel _viewModel = new PlacementInformationViewModel();

        private const int OpportunityId = 1;

        public When_Placement_Information_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;
            _viewModel.JobTitle = JobTitle;
            _viewModel.PlacementsKnown = PlacementsKnown;
            _viewModel.Placements = Placements;

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(_dto);

            var tempData = Substitute.For<ITempDataDictionary>();
            var opportunityController = new OpportunityController(_opportunityService);
            opportunityController.AddUsernameToContext(ModifiedBy);

            opportunityController.TempData = tempData;
            opportunityController.Placements(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(_dto);
        }

        [Fact]
        public void Then_JobTitle_Is_Populated()
        {
            _dto.JobTitle.Should().Be(JobTitle);
        }

        [Fact]
        public void Then_PlacementsKnown_Is_Populated()
        {
            _dto.PlacementsKnown.Should().Be(PlacementsKnown);
        }

        [Fact]
        public void Then_Placements_Is_Populated()
        {
            _dto.Placements.Should().Be(Placements);
        }

        [Fact]
        public void Then_ModifiedBy_Is_Populated()
        {
            _dto.ModifiedBy.Should().Be(ModifiedBy);
        }
    }
}