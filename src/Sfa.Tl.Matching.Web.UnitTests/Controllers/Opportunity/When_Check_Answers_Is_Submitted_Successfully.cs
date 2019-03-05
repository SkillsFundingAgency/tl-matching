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
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswers(new CheckAnswersViewModel { OpportunityId = OpportunityId, ConfirmationSelected = true });
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