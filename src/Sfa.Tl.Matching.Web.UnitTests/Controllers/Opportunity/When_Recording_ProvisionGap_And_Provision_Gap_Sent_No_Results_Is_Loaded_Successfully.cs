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
    public class When_Recording_ProvisionGap_And_Provision_Gap_Sent_No_Results_Is_Loaded_Successfully
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Recording_ProvisionGap_And_Provision_Gap_Sent_No_Results_Is_Loaded_Successfully()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(SentViewModelMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityService = Substitute.For<IOpportunityService>();

            var dto = new ValidOpportunityDtoBuilder().Build();
            dto.SearchResultProviderCount = 0;
            _opportunityService.GetOpportunity(1).Returns(dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName("CreatedBy")
                .Build();

            _result = controllerWithClaims.ProvisionGapSent(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateProvisionGap_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(1);
        }

        [Fact]
        public void Then_Result_Is_ViewResult()
        {
            var viewResult = _result as ViewResult;

            viewResult.Should().NotBeNull();

            viewResult?.Model.Should().BeOfType<ProvisionGapSentViewModel>();

            ((ProvisionGapSentViewModel)viewResult?.Model)?.EmployerContact.Should().Be("EmployerContact");
        }

        [Fact]
        public void Then_ViewModel_Properties_Are_Set()
        {
            var viewModel = _result.GetViewModel<ProvisionGapSentViewModel>();
            viewModel.EmployerContact.Should().Be("EmployerContact");
            viewModel.RouteId.Should().Be(1);
            viewModel.SearchRadius.Should().Be(3);
            viewModel.Postcode.Should().Be("AA1 1AA");
            viewModel.UserEmail.Should().Be("email@address.com");
            viewModel.JobTitle.Should().Be("JobTitle");
            viewModel.Placements.Should().Be(2);
            viewModel.EmployerName.Should().Be("EmployerName");
            viewModel.EmployerCrmRecord.Should().Be("https://crm.employer.imservices.org.uk/EmployerCRM/main.aspx?etc=1&extraqs=formid%3d53e2f137-d7f8-4556-a260-bd320fa7e62c&id=%7b65021261-8c70-4c4f-954f-4e5282250a85%7d&pagetype=entityrecord");
            viewModel.WithResults.Should().BeFalse();
            viewModel.NoResults.Should().BeTrue();
        }
    }
}