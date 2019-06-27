using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityEmployerName_Is_Submitted_Invalid_Employer
    {
        private readonly IActionResult _result;
        private readonly EmployerController _employerController;

        public When_Employer_SaveOpportunityEmployerName_Is_Submitted_Invalid_Employer()
        {
            var employerService = Substitute.For<IEmployerService>();
            employerService.GetEmployer(Arg.Any<int>())
                .ReturnsNull();
            var opportunityService = Substitute.For<IOpportunityService>();

            var viewModel = new FindEmployerViewModel
            {
                CompanyName = "Invalid Business Name"
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerStagingMapper).Assembly));
            var mapper = new Mapper(config);

            _employerController = new EmployerController(employerService, opportunityService, mapper);

            _result = _employerController.SaveOpportunityEmployerName(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _employerController.ViewData.ModelState.Should().ContainSingle();

        [Fact]
        public void Then_Model_State_Has_CompanyName_Key() =>
            _employerController.ViewData.ModelState.ContainsKey(nameof(FindEmployerViewModel.CompanyName))
                .Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_CompanyName_Error()
        {
            var modelStateEntry = _employerController.ViewData.ModelState[nameof(FindEmployerViewModel.CompanyName)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must find and choose an employer");
        }
    }
}