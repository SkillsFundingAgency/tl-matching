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
    public class When_Placement_Information_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string ModifiedBy = "ModifiedBy";
        private readonly PlacementInformationViewModel _viewModel = new PlacementInformationViewModel();

        private const int OpportunityId = 1;

        public When_Placement_Information_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService);
            opportunityController.AddUsernameToContext(ModifiedBy);

            opportunityController.Placements(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(Arg.Any<OpportunityDto>());
        }
    }
    public class When_Check_Answers_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "ModifiedBy";
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Check_Answers_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService);
            opportunityController.AddUsernameToContext(CreatedBy);

            _result = opportunityController.CheckAnswers(new CheckAnswersViewModel { OpportunityId = OpportunityId, ConfirmationSelected = true });
        }

        [Fact]
        public void Then_CreateProvisionGap_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateProvisionGap(Arg.Any<CheckAnswersViewModel>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_to_PlacementGap()
        {
            var result = _result as RedirectToActionResult;
            result.Should().NotBeNull();

            result?.ActionName.Should().Be("PlacementGap");
        }
    }
}
