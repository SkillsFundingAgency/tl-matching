using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_ProvisionGap_And_Check_Answers_Gap_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string ModifiedBy = "ModifiedBy";
        private readonly IActionResult _result;
        private const bool ConfirmationSelected = true;

        private readonly OpportunityDto _dto = new OpportunityDto
        {
            Id = OpportunityId,
            ConfirmationSelected = false
        };

        private const int OpportunityId = 1;

        public When_Recording_ProvisionGap_And_Check_Answers_Gap_Is_Submitted_Successfully()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(new OpportunityDto { Id = OpportunityId, ConfirmationSelected = false });

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(ModifiedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswersProvisionGap(new CheckAnswersProvisionGapViewModel
            {
                OpportunityId = OpportunityId,
                ConfirmationSelected = true
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_SaveCheckAnswers_Is_Called_Exectlt_Once()
        {
            _opportunityService.Received(1).SaveCheckAnswers(Arg.Is<CheckAnswersDto>(a =>
                a.ConfirmationSelected == ConfirmationSelected &&
                a.ModifiedBy == ModifiedBy));
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