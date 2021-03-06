﻿using System;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_Referrals_And_Emails_Sent_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;
        private readonly Guid _employerCrmId;

        public When_Recording_Referrals_And_Emails_Sent_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(SentViewModelMapper).Assembly));

            var mapper = new Mapper(config);

            var dto = new ValidOpportunityDtoBuilder().Build();
            _employerCrmId = dto.EmployerCrmId;

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityAsync(1).Returns(dto);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.GetReferralEmailSentAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunityAsync(1);
        }

        [Fact]
        public void Then_ViewModel_Properties_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<SentViewModel>();
            viewModel.PrimaryContact.Should().Be("EmployerContact");
            viewModel.CompanyName.Should().Be("CompanyName");
            viewModel.EmployerCrmRecord.Should().Be($"https://esfa-cs-prod.crm4.dynamics.com/main.aspx?pagetype=entityrecord&etc=1&id=%7b{_employerCrmId}%7d&extraqs=&newWindow=true");
        }
    }
}