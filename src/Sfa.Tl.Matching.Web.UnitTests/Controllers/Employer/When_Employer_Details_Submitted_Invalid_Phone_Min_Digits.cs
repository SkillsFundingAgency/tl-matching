using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Details_Submitted_Invalid_Phone_Min_Digits : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;
        
        public When_Employer_Details_Submitted_Invalid_Phone_Min_Digits(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;
            
            var viewModel = new EmployerDetailsViewModel
            {
                Phone = "1"
            };

            _result = _fixture.Sut.SaveOpportunityEmployerDetailsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
        }

        [Fact]
        public void Then_Model_State_Has_ContactPhone_Error()
        {
            _fixture.Sut.ViewData.ModelState.Should().ContainSingle();

            _fixture.Sut.ViewData.ModelState.ContainsKey(nameof(EmployerDetailsViewModel.Phone))
                .Should().BeTrue();

            var modelStateEntry = _fixture.Sut.ViewData.ModelState[nameof(EmployerDetailsViewModel.Phone)];
            
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must enter a telephone number that has 7 or more numbers");
        }
    }
}