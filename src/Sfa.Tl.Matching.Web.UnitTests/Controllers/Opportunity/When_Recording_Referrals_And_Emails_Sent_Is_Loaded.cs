﻿using AutoMapper;
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

        public When_Recording_Referrals_And_Emails_Sent_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(SentViewModelMapper).Assembly));

            var mapper = new Mapper(config);

            var dto = new ValidOpportunityDtoBuilder().Build();
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(1).Returns(dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.ReferralEmailSent(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(1);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_ViewModel_Properties_Are_Set()
        {
            var viewModel = _result.GetViewModel<EmailsSentViewModel>();
            viewModel.EmployerContact.Should().Be("EmployerContact");
            viewModel.RouteId.Should().Be(1);
            viewModel.SearchRadius.Should().Be(3);
            viewModel.Postcode.Should().Be("AA1 1AA");
            viewModel.UserEmail.Should().Be("email@address.com");
            viewModel.JobTitle.Should().Be("JobTitle");
            viewModel.Placements.Should().Be(2);
            viewModel.EmployerName.Should().Be("EmployerName");
            viewModel.EmployerCrmRecord.Should().Be("https://crm.employer.imservices.org.uk/EmployerCRM/main.aspx?etc=1&extraqs=formid%3d53e2f137-d7f8-4556-a260-bd320fa7e62c&id=%7b65021261-8c70-4c4f-954f-4e5282250a85%7d&pagetype=entityrecord");
        }
    }
}