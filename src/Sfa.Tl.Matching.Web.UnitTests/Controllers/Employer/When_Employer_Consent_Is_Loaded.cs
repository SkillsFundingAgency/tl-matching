using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Consent_Is_Loaded
    {
        private readonly IActionResult _result;

        private readonly IEmployerService _employerService;
        private readonly IOpportunityService _opportunityService;

        public When_Employer_Consent_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var referralService = Substitute.For<IReferralService>();
            var mapper = new Mapper(config);

            _employerService = Substitute.For<IEmployerService>();
            _employerService.GetOpportunityEmployerDetailAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new EmployerDetailsViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                CompanyName = "CompanyName",
                AlsoKnownAs = "CompanyNameAka",
                PrimaryContact = "EmployerContact",
                Phone = "EmployerContactPhone",
                Email = "EmployerContactEmail"
            });

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetReferredOpportunityItemCountAsync(1).Returns(10);

            var employerController = new EmployerController(_employerService, _opportunityService, referralService, mapper);

            _result = employerController.GetEmployerConsentAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmployerDetailsViewModel_Is_Populated_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<EmployerConsentViewModel>();

            viewModel.OpportunityId.Should().Be(1);
            viewModel.OpportunityItemId.Should().Be(2);
            viewModel.OpportunityItemCount.Should().Be(10);
            viewModel.ConfirmationSelected.Should().BeFalse();

            viewModel.Details.CompanyName.Should().Be("CompanyName");
            viewModel.Details.AlsoKnownAs.Should().Be("CompanyNameAka");
            viewModel.Details.CompanyNameWithAka.Should().Be("CompanyName (CompanyNameAka)");
            viewModel.Details.PrimaryContact.Should().Be("EmployerContact");
            viewModel.Details.Email.Should().Be("EmployerContactEmail");
            viewModel.Details.Phone.Should().Be("EmployerContactPhone");
        }

        [Fact]
        public void Then_GetReferredOpportunityItemCountAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetReferredOpportunityItemCountAsync(1);
        }

        [Fact]
        public void Then_GetOpportunityEmployerDetailAsync_Is_Called_Exactly_Once()
        {
            _employerService
                .Received(1)
                .GetOpportunityEmployerDetailAsync(1, 2);
        }
    }
}