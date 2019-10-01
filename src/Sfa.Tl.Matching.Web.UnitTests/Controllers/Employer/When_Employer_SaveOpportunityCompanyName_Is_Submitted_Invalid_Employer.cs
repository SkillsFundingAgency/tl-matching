using System;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_Invalid_Employer
    {
        private readonly IActionResult _result;
        private readonly EmployerController _employerController;

        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_Invalid_Employer()
        {
            var employerService = Substitute.For<IEmployerService>();
            employerService.ValidateCompanyNameAndCrmIdAsync(Arg.Any<Guid>(), Arg.Any<string>())
                .Returns(false);
            var opportunityService = Substitute.For<IOpportunityService>();

            var viewModel = new FindEmployerViewModel
            {
                CompanyName = "Invalid Business Name"
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var referralService = Substitute.For<IReferralService>();
            var mapper = new Mapper(config);

            var employerController = new EmployerController(employerService, opportunityService, referralService, mapper);

            _employerController = new ClaimsBuilder<EmployerController>(employerController)
                .Add(ClaimTypes.Role, RolesExtensions.StandardUser)
                .AddUserName("Username")
                .Build();

            _result = _employerController.SaveOpportunityCompanyName(viewModel).GetAwaiter().GetResult();
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