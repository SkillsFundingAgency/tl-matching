using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
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

        private const int OpportunityId = 1;

        public When_Recording_ProvisionGap_And_Check_Answers_Gap_Is_Submitted_Successfully()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(CheckAnswersDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<CheckAnswersProvisionGapViewModel, CheckAnswersDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<CheckAnswersProvisionGapViewModel, CheckAnswersDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<CheckAnswersProvisionGapViewModel, CheckAnswersDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);
            
            _opportunityService = Substitute.For<IOpportunityService>();
             
			 var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(ModifiedBy)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.CheckAnswersProvisionGap(new CheckAnswersProvisionGapViewModel
            {
                OpportunityId = OpportunityId,
                ConfirmationSelected = true
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_SaveCheckAnswers_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(Arg.Is<CheckAnswersDto>(a =>
                a.ConfirmationSelected == ConfirmationSelected));
        }

        [Fact]
        public void Then_CreateProvisionGap_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateProvisionGap(Arg.Is<CheckAnswersProvisionGapViewModel>(a =>
                a.ConfirmationSelected == ConfirmationSelected));
        }

        [Fact]
        public void Then_Result_Is_Redirected_to_Provision_Gap_Sent()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("ProvisionGapSent_Get");
        }
    }
}