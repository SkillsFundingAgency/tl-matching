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

        private readonly EmployerDetailsViewModel _viewModel = new()
        {
            OpportunityItemId = OpportunityItemId,
            OpportunityId = OpportunityId,
            PrimaryContact = Contact,
            Email = ContactEmail,
            Phone = ContactPhone
        };

        private const int OpportunityId = 1;
        private const int OpportunityItemId = 2;

        private readonly IActionResult _result;

        public When_Recording_ProvisionGap_And_Employer_Details_Is_Submitted_Successfully()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.IsReferralOpportunityItemAsync(OpportunityId).Returns(false);

            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            var referralService = Substitute.For<IReferralService>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<EmployerDetailsViewModel, EmployerDetailDto>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            new LoggedInUserNameResolver<EmployerDetailsViewModel, EmployerDetailDto>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<EmployerDetailsViewModel, EmployerDetailDto>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var employerController = new EmployerController(null, _opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(employerController)
                .AddStandardUser()
                .AddUserName(ModifiedBy)
                .Build();

            httpContextAccessor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveOpportunityEmployerDetailsAsync(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).IsReferralOpportunityItemAsync(OpportunityItemId);
        }

        [Fact]
        public void Then_SaveEmployerDetail_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunityAsync(Arg.Is<EmployerDetailDto>(a =>
                a.PrimaryContact == Contact &&
                a.Email == ContactEmail &&
                a.Phone == ContactPhone &&
                a.ModifiedBy == ModifiedBy));
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SaveCheckAnswers()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToActionResult>(); 
            var redirect = _result as RedirectToActionResult;
            redirect.Should().NotBeNull();
            redirect?.ControllerName.Should().BeEquivalentTo("Opportunity");
            redirect?.ActionName.Should().BeEquivalentTo("SaveCheckAnswers");
            redirect?.RouteValues["opportunityId"].Should().Be(1);
            redirect?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}