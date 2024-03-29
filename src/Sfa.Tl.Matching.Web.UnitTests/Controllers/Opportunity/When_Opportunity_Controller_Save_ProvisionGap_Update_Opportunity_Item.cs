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
    public class When_Opportunity_Controller_Save_ProvisionGap_Update_Opportunity
    {
        private readonly IOpportunityService _opportunityService;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_ProvisionGap_Update_Opportunity()
        {
            const int opportunityId = 1;
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.IsNewProvisionGapAsync(opportunityId).Returns(false);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<SaveProvisionGapViewModel, OpportunityDto>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            new LoggedInUserNameResolver<SaveProvisionGapViewModel, OpportunityDto>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<SaveProvisionGapViewModel, OpportunityDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveProvisionGapAsync(new SaveProvisionGapViewModel { SearchResultProviderCount = 0, SelectedRouteId = 1, Postcode = "cv12wt", SearchRadius = 10 }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceive()
                .CreateOpportunityAsync(Arg.Any<OpportunityDto>());
        }

        [Fact]
        public void Then_CreateOpportunityItem_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceive()
                .CreateOpportunityItemAsync(Arg.Any<OpportunityItemDto>());
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .UpdateOpportunityItemAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetPlacementInformation()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetPlacementInformation");
        }
    }
}