﻿using System.Security.Claims;
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
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_And_Employer_Is_Already_In_Opportunity
    {
        private readonly IActionResult _result;
        private readonly EmployerController _employerController;

        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_And_Employer_Is_Already_In_Opportunity()
        {
            var employerService = Substitute.For<IEmployerService>();
            employerService.ValidateCompanyNameAndId(Arg.Any<int>(), Arg.Any<string>())
                .Returns(true);
            employerService.GetEmployerOpportunityOwnerAsync(Arg.Any<int>())
                .Returns("Another User");
            var opportunityService = Substitute.For<IOpportunityService>();

            var viewModel = new FindEmployerViewModel
            {
                OpportunityId = 1,
                CompanyName = "Company Name"
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerStagingMapper).Assembly));
            var mapper = new Mapper(config);

            _employerController = new EmployerController(employerService, opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<EmployerController>(_employerController)
                .Add(ClaimTypes.Role, RolesExtensions.StandardUser)
                .AddUserName("Current User")
                .Build();

            _result = controllerWithClaims.SaveOpportunityCompanyName(viewModel).GetAwaiter().GetResult();
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
            modelStateEntry
                .Errors[0]
                .ErrorMessage
                .Should()
                .Be("Another User is already working on this employer’s opportunities. " +
                    "Please choose a different employer.");
        }
    }
}