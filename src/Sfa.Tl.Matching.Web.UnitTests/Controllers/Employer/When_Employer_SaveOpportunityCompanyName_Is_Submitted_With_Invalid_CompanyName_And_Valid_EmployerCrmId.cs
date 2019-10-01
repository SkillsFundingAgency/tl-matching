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
    public class When_Employer_SaveOpportunityCompanyName_Is_Submitted_With_Invalid_CompanyName_And_Valid_EmployerCrmId
    {
        private readonly EmployerController _employerController;

        public When_Employer_SaveOpportunityCompanyName_Is_Submitted_With_Invalid_CompanyName_And_Valid_EmployerCrmId()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityService = Substitute.For<IOpportunityService>();
            var employerService = Substitute.For<IEmployerService>();
            var referralService = Substitute.For<IReferralService>();

            employerService.ValidateCompanyNameAndCrmIdAsync(new Guid("11111111-1111-1111-1111-111111111111"), "").Returns(false);

            var employerController = new EmployerController(employerService, opportunityService, referralService, mapper);

            _employerController = new ClaimsBuilder<EmployerController>(employerController)
                .Add(ClaimTypes.Role, RolesExtensions.StandardUser)
                .AddUserName("Username")
                .Build();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("Invalid Business Name")]
        public void Then_View_Result_Is_Returned_With_Model_State_Error_For_CompanyName(string companyName)
        {
            var result = _employerController.SaveOpportunityCompanyNameAsync(new FindEmployerViewModel { CompanyName = companyName, SelectedEmployerCrmId = new Guid("11111111-1111-1111-1111-111111111111") }).GetAwaiter().GetResult();

            result.Should().BeAssignableTo<ViewResult>();
            _employerController.ViewData.ModelState.Should().ContainSingle();
            _employerController.ViewData.ModelState.ContainsKey(nameof(FindEmployerViewModel.CompanyName)).Should().BeTrue();
            var modelStateEntry = _employerController.ViewData.ModelState[nameof(FindEmployerViewModel.CompanyName)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must find and choose an employer");
        }
    }
}