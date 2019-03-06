using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Check_Answers_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Check_Answers_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            var viewModel = new CheckAnswersViewModel
            {
                OpportunityId = OpportunityId,
                ConfirmationSelected = true
            };

            _result = controllerWithClaims.CheckAnswers(viewModel).GetAwaiter().GetResult();
        }
    }
}