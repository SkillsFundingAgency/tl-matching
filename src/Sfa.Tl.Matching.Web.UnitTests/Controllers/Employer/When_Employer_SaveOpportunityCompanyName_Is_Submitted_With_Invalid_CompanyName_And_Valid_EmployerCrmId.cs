using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_With_Invalid_CompanyName_And_Valid_EmployerCrmId : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly EmployerController ControllerWithClaims;
        
        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_With_Invalid_CompanyName_And_Valid_EmployerCrmId(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.EmployerService.ValidateCompanyNameAndCrmIdAsync(_fixture.EmployerCrmId, "").Returns(false);

            ControllerWithClaims = _fixture.Sut.ControllerWithClaims("Username");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("Invalid Business Name")]
        public void Then_View_Result_Is_Returned_With_Model_State_Error_For_CompanyName(string companyName)
        {
            var result = ControllerWithClaims.SaveOpportunityCompanyNameAsync(new FindEmployerViewModel
                {CompanyName = companyName, SelectedEmployerCrmId = _fixture.EmployerCrmId}).GetAwaiter().GetResult();

            result.Should().BeAssignableTo<ViewResult>();
            ControllerWithClaims.ViewData.ModelState.Should().ContainSingle();
            ControllerWithClaims.ViewData.ModelState.ContainsKey(nameof(FindEmployerViewModel.CompanyName)).Should().BeTrue();

            var modelStateEntry = ControllerWithClaims.ViewData.ModelState[nameof(FindEmployerViewModel.CompanyName)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must find and choose an employer");
        }
    }
}