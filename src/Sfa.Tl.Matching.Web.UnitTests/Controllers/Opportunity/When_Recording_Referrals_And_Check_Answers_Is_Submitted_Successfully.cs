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
    public class When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;
        private readonly CheckAnswersReferralViewModel _viewModel = new CheckAnswersReferralViewModel();

        public When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswersReferrals(_viewModel).GetAwaiter().GetResult();
        }

        // TODO AU This should be updating the opportunity
        //[Fact]
        //public void Then_CreateReferral_Is_Called_Exactly_Once()
        //{
        //    _opportunityService.Received(1).CreateReferral(_viewModel);
        //}

        [Fact]
        public void Then_Result_Is_Redirect_to_EmailsSent()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("EmailSentReferrals_Get");
        }
    }
}