﻿using AutoMapper;
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
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 2;

        private readonly CheckAnswersViewModel _viewModel = new()
        {
            OpportunityId = OpportunityId,
            OpportunityItemId = OpportunityItemId
        };

        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(CheckAnswersDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<CheckAnswersViewModel, CheckAnswersDto>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            new LoggedInUserNameResolver<CheckAnswersViewModel, CheckAnswersDto>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<CheckAnswersViewModel, CheckAnswersDto>(new DateTimeProvider()) :
                                null);
            });

            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityController = new OpportunityController(_opportunityService,  mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(ModifiedBy)
                .Build();

            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveCheckAnswers(_viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetOpportunityBasket");
            result?.RouteValues["opportunityId"].Should().Be(1);
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }

        [Fact]
        public void Then_UpdateOpportunityItemAsync_Is_Called_Exactly_Once()
        {
            // TODO Assert args
            _opportunityService.Received(1).UpdateOpportunityItemAsync(Arg.Any<CheckAnswersDto>());
        }
    }
}