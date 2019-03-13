using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_ProvisionGap_And_Check_Answers_Gap_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;
        private readonly CheckAnswersProvisionGapViewModel _viewModel = new CheckAnswersProvisionGapViewModel();

        public When_Recording_ProvisionGap_And_Check_Answers_Gap_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswersProvisionGap(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateProvisionGap_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateProvisionGap(_viewModel);
        }

        [Fact]
        public void Then_Result_Is_Redirect_to_PlacementGap()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("EmailSentProvisionGap_Get");
        }
    }
}