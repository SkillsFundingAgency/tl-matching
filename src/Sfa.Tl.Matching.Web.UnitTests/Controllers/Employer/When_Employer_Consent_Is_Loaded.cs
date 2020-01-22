using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Consent_Is_Loaded : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;


        public When_Employer_Consent_Is_Loaded(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.EmployerService.GetOpportunityEmployerDetailAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new EmployerDetailsViewModel
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                CompanyName = "CompanyName",
                AlsoKnownAs = "CompanyNameAka",
                PrimaryContact = "EmployerContact",
                Phone = "EmployerContactPhone",
                Email = "EmployerContactEmail"
            });

            _fixture.OpportunityService.GetReferredOpportunityItemCountAsync(1).Returns(10);

            _result = _fixture.Sut.GetEmployerConsentAsync(1, 2).GetAwaiter().GetResult();
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
            _fixture.OpportunityService
                .Received(1)
                .GetReferredOpportunityItemCountAsync(1);
        }

        [Fact]
        public void Then_GetOpportunityEmployerDetailAsync_Is_Called_Exactly_Once()
        {
            _fixture.EmployerService
                .Received(2)
                .GetOpportunityEmployerDetailAsync(1, 2);
        }
    }
}