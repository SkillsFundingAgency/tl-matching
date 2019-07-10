using System;
using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Controller_Delete_Employer
    {
        private IActionResult _result;
        private const int OpportunityId = 12;
        private readonly IEmployerService _employerService;
        private readonly EmployerController _employerController;
        
        public When_Employer_Controller_Delete_Employer()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            _employerService = Substitute.For<IEmployerService>();
            
            _employerController = new EmployerController(_employerService, Substitute.For<IOpportunityService>(), mapper)
            {
                ControllerContext = new ControllerContext()
            };

            _employerController.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [Fact]
        public void Then_Result_Should_Return_To_GetSavedEmployerOpportunity()
        {
            _employerService.GetSavedEmployerOpportunitiesAsync(Arg.Any<string>()).Returns(new SavedEmployerOpportunityViewModel
            {
                EmployerOpportunities = new List<EmployerOpportunityViewModel>
                {
                    new EmployerOpportunityViewModel
                    {
                        OpportunityId = 12,
                        Name = "Test A Company Name",
                        LastUpdated = DateTime.Now
                    }
                }
            });

            _result = _employerController.DeleteEmployer(OpportunityId).GetAwaiter().GetResult();

            var routename = _result as RedirectToRouteResult;

            routename.Should().NotBeNull();
            routename?.RouteName.Should().Be("GetSavedEmployerOpportunity");
            routename?.RouteName.Should().NotBe("Start");

        }

        [Fact]
        public void Then_Result_Should_Return_To_Start()
        {
            _employerService.GetSavedEmployerOpportunitiesAsync(Arg.Any<string>()).Returns(new SavedEmployerOpportunityViewModel
            {
                EmployerOpportunities = new List<EmployerOpportunityViewModel>()
            });

            _result = _employerController.DeleteEmployer(OpportunityId).GetAwaiter().GetResult();

            var routename = _result as RedirectToRouteResult;

            routename.Should().NotBeNull();
            routename?.RouteName.Should().Be("Start");
            routename?.RouteName.Should().NotBe("GetSavedEmployerOpportunity");
            
        }

    }
}
