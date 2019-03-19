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
    public class When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully
    {
        private const string ModifiedBy = "ModifiedBy";
        private readonly IActionResult _result;
        private readonly CheckAnswersReferralViewModel _viewModel = new CheckAnswersReferralViewModel();

        public When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(CheckAnswersDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<CheckAnswersReferralViewModel, CheckAnswersDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<CheckAnswersReferralViewModel, CheckAnswersDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<CheckAnswersReferralViewModel, CheckAnswersDto>(new DateTimeProvider()) :
                                null);
            });

            var mapper = new Mapper(config);

            var opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(ModifiedBy)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.CheckAnswersReferrals(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_to_EmailsSent()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("EmailSentReferrals_Get");
        }
    }
}