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
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_PlacementGap_And_Email_Sent_Is_Loaded_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;
        private const string CreatedBy = "ModifiedBy";
        private const string EmployerContact = "Hardik";

        private const int OpportunityId = 1;

        public When_Recording_PlacementGap_And_Email_Sent_Is_Loaded_Successfully()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityService = Substitute.For<IOpportunityService>();

            _opportunityService.GetOpportunity(OpportunityId).Returns(new OpportunityDto { Id = OpportunityId, EmployerContact = EmployerContact });

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.EmailSentProvisionGap(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateProvisionGap_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }

        [Fact]
        public void Then_Result_Is_ViewResult()
        {
            var viewResult = _result as ViewResult;

            viewResult.Should().NotBeNull();

            viewResult?.Model.Should().BeOfType<EmailSentProvisionGapViewModel>();

            ((EmailSentProvisionGapViewModel)viewResult?.Model)?.EmployerContactName.Should().Be(EmployerContact);
        }

        [Fact]
        public void Then_EmployerContactName_Is_Set()
        {
            var viewModel = _result.GetViewModel<EmailSentProvisionGapViewModel>();
            viewModel.EmployerContactName.Should().Be(EmployerContact);
        }
    }
}