using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_And_Employer_Is_Already_In_Opportunity : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;
        
        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_And_Employer_Is_Already_In_Opportunity(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.EmployerService.ValidateCompanyNameAndCrmIdAsync(Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(true);

            _fixture.EmployerService.GetEmployerOpportunityOwnerAsync(Arg.Any<Guid>())
                .Returns("Another User");

            var viewModel = new FindEmployerViewModel
            {
                OpportunityId = 1,
                CompanyName = "Company Name"
            };

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("Current User");

            _result = controllerWithClaims.SaveOpportunityCompanyNameAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
        }

        [Fact]
        public void Then_Model_State_Has_CompanyName_Error()
        {
            _fixture.Sut.ViewData.ModelState.Should().ContainSingle();

            _fixture.Sut.ViewData.ModelState.ContainsKey(nameof(FindEmployerViewModel.CompanyName))
                    .Should().BeTrue();

            var modelStateEntry = _fixture.Sut.ViewData.ModelState[nameof(FindEmployerViewModel.CompanyName)];

            modelStateEntry
                .Errors[0]
                .ErrorMessage
                .Should()
                .Be("Your colleague, Another User, is already working on this employer’s opportunities. " +
                    "Please choose a different employer.");
        }
    }
}