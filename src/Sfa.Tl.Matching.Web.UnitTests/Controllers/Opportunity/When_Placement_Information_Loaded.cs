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
    public class When_Placement_Information_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        private readonly OpportunityDto _dto = new OpportunityDto();
        private const string JobTitle = "JobTitle";
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const int OpportunityId = 12;

        public When_Placement_Information_Loaded()
        {
            _dto.Id = OpportunityId;
            _dto.JobTitle = JobTitle;
            _dto.PlacementsKnown = PlacementsKnown;
            _dto.Placements = Placements;

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(_dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService);

            _result = opportunityController.PlacementInformationSave(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_OpportunityId_Is_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        [Fact]
        public void Then_JobTitle_Is_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.JobTitle.Should().Be(JobTitle);
        }

        [Fact]
        public void Then_PlacementsKnown_Is_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.PlacementsKnown.Should().Be(PlacementsKnown);
        }

        [Fact]
        public void Then_Placements_Is_Set()
        {
            var viewModel = _result.GetViewModel<PlacementInformationSaveViewModel>();
            viewModel.Placements.Should().Be(Placements);
        }
    }
}