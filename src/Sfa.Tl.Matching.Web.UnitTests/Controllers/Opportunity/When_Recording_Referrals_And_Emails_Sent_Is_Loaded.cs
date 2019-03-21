using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_Referrals_And_Emails_Sent_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;

        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Recording_Referrals_And_Emails_Sent_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));

            var mapper = new Mapper(config);

            var dto = new ValidOpportunityDtoBuilder().Build();
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.EmailSentReferrals(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
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
        public void Then_EmployerBusinessName_Is_Set()
        {
            var viewModel = _result.GetViewModel<EmailsSentViewModel>();
            viewModel.EmployerBusinessName.Should().Be("CompanyName");
        }

        [Fact]
        public void Then_EmployerContactName_Is_Set()
        {
            var viewModel = _result.GetViewModel<EmailsSentViewModel>();
            viewModel.EmployerContactName.Should().Be("EmployerContact");
        }
    }
}