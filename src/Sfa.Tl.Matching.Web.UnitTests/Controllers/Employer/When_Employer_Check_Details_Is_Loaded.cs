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
    public class When_Employer_Check_Details_Is_Loaded : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Employer_Check_Details_Is_Loaded(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.EmployerService.GetOpportunityEmployerDetailAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new EmployerDetailsViewModel
            {
                OpportunityId = _fixture.OpportunityId,
                OpportunityItemId = _fixture.OpportunityItemId,
                CompanyName = _fixture.CompanyName,
                PrimaryContact = _fixture.EmployerContact,
                Phone = _fixture.EmployerContactPhone,
                Email = _fixture.EmployerContactEmail
            });

            _result = _fixture.Sut.CheckEmployerDetailsAsync(_fixture.OpportunityId, _fixture.OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmployerDetailsViewModel_Is_Populated_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<EmployerDetailsViewModel>();
            viewModel.Should().NotBeNull();

            viewModel.OpportunityItemId.Should().Be(_fixture.OpportunityItemId);
            viewModel.OpportunityId.Should().Be(_fixture.OpportunityId);
            viewModel.CompanyName.Should().Be(_fixture.CompanyName);
            viewModel.PrimaryContact.Should().Be(_fixture.EmployerContact);
            viewModel.Phone.Should().Be(_fixture.EmployerContactPhone);
            viewModel.Email.Should().Be(_fixture.EmployerContactEmail);
        }
    }
}