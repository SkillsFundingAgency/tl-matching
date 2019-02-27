using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_FindEmployer_Is_Submitted_Invalid_Employer
    {
        private readonly IActionResult _result;
        private readonly EmployerController _employerController;

        public When_Employer_FindEmployer_Is_Submitted_Invalid_Employer()
        {
            var employerService = Substitute.For<IEmployerService>();
            employerService.GetEmployer(Arg.Any<int>())
                .ReturnsNull();
            var opportunityService = Substitute.For<IOpportunityService>();

            var viewModel = new FindEmployerViewModel
            {
                BusinessName = "Invalid Business Name"
            };

            _employerController = new EmployerController(employerService, opportunityService);

            _result = _employerController.FindEmployer(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _employerController.ViewData.ModelState.Should().ContainSingle();

        [Fact]
        public void Then_Model_State_Has_BusinessName_Key() =>
            _employerController.ViewData.ModelState.ContainsKey(nameof(FindEmployerViewModel.BusinessName))
                .Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_BusinessName_Error()
        {
            var modelStateEntry = _employerController.ViewData.ModelState[nameof(FindEmployerViewModel.BusinessName)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must find and choose an employer");
        }
    }
}