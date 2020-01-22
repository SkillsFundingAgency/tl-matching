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
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_Invalid_Employer : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;
        
        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_Invalid_Employer(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.EmployerService.ValidateCompanyNameAndCrmIdAsync(Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(false);

            var viewModel = new FindEmployerViewModel
            {
                CompanyName = "Invalid Business Name"
            };

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("Username");

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

            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must find and choose an employer");
        }
    }
}