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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Recording_ProvisionGap_And_Employer_Details_Is_Submitted_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private const string Contact = "EmployerContact";
        private const string ContactPhone = "123456789";
        private const string ContactEmail = "EmployerContactEmail";
        private const string ModifiedBy = "ModifiedBy";

        private readonly EmployerDetailsViewModel _viewModel = new EmployerDetailsViewModel
        {
            OpportunityId = OpportunityId,
            EmployerContact = Contact,
            EmployerContactEmail = ContactEmail,
            EmployerContactPhone = ContactPhone
        };

        private const int OpportunityId = 1;

        private readonly IActionResult _result;

        public When_Recording_ProvisionGap_And_Employer_Details_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.IsReferralOpportunity(OpportunityId).Returns(false);

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type => 
                    type.Name.Contains("LoggedInUserEmailResolver") ? 
                        new LoggedInUserEmailResolver<EmployerDetailsViewModel, EmployerDetailDto>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ? 
                            (object) new LoggedInUserNameResolver<EmployerDetailsViewModel, EmployerDetailDto>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ? 
                                new UtcNowResolver<EmployerDetailsViewModel, EmployerDetailDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var employerController = new EmployerController(null, _opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddStandardUser()
                .AddUserName(ModifiedBy)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.Details(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).IsReferralOpportunity(OpportunityId);
        }

        [Fact]
        public void Then_SaveEmployerDetail_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).SaveEmployerDetail(Arg.Is<EmployerDetailDto>(a =>
                a.EmployerContact == Contact &&
                a.EmployerContactEmail == ContactEmail &&
                a.EmployerContactPhone == ContactPhone &&
                a.ModifiedBy == ModifiedBy));
        }

        [Fact]
        public void Then_Result_Is_RedirectResult() =>
            _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("CheckAnswersProvisionGap_Get");
        }
    }
}