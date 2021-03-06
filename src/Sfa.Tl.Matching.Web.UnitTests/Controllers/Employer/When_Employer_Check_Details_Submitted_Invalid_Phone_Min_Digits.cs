﻿using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Check_Details_Submitted_Invalid_Phone_Min_Digits
    {
        private readonly IActionResult _result;
        private readonly EmployerController _employerController;

        public When_Employer_Check_Details_Submitted_Invalid_Phone_Min_Digits()
        {
            var employerService = Substitute.For<IEmployerService>();
            var opportunityService = Substitute.For<IOpportunityService>();
            var referralService = Substitute.For<IReferralService>();

            var viewModel = new EmployerDetailsViewModel
            {
                Phone = "1"
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            _employerController = new EmployerController(employerService, opportunityService, referralService, mapper);

            _result = _employerController.SaveCheckOpportunityEmployerDetailsAsync(viewModel).GetAwaiter().GetResult();
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
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            _employerController.ViewData.ModelState.Should().ContainSingle();
            _employerController.ViewData.ModelState.ContainsKey(nameof(EmployerDetailsViewModel.Phone))
                .Should().BeTrue();

            var modelStateEntry =
                _employerController.ViewData.ModelState[nameof(EmployerDetailsViewModel.Phone)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("You must enter a telephone number that has 7 or more numbers");
        }
    }
}