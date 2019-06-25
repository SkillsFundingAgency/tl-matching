using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Details_Is_Loaded_With_Existing_Details
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;
        private readonly IEmployerService _employerService;

        private const int OpportunityId = 12;
        private const string CompanyName = "CompanyName";
        private const string EmployerContact = "EmployerContact";
        private const string EmployerContactPhone = "EmployerContactPhone";
        private const string EmployerContactEmail = "EmployerContactEmail";

        public When_Employer_Details_Is_Loaded_With_Existing_Details()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            _employerService = Substitute.For<IEmployerService>();
            _employerService.GetEmployer(Arg.Any<int>()).Returns(new EmployerStagingDto
            {
                CompanyName = CompanyName
            });
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(new OpportunityDto
            {
                Id = OpportunityId,
                //EmployerName = EmployerName,
                EmployerContact = EmployerContact,
                EmployerContactPhone = EmployerContactPhone,
                EmployerContactEmail = EmployerContactEmail
            });

            var employerController = new EmployerController(_employerService, _opportunityService, mapper);

            _result = employerController.Details(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_OpportunityId_Is_Set()
        {
            var viewModel = _result.GetViewModel<EmployerDetailsViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        [Fact]
        public void Then_EmployerName_Is_Populated()
        {
            var viewModel = _result.GetViewModel<EmployerDetailsViewModel>();
            viewModel.EmployerName.Should().Be(CompanyName);
        }

        [Fact]
        public void Then_EmployerContactPhone_Is_Populated()
        {
            var viewModel = _result.GetViewModel<EmployerDetailsViewModel>();
            viewModel.EmployerContactPhone.Should().Be(EmployerContactPhone);
        }

        [Fact]
        public void Then_EmployerContactEmail_Is_Populated()
        {
            var viewModel = _result.GetViewModel<EmployerDetailsViewModel>();
            viewModel.EmployerContactEmail.Should().Be(EmployerContactEmail);
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }
        [Fact]
        public void Then_GetEmployer_Is_Not_Called()
        {
            _employerService.DidNotReceive().GetEmployer(Arg.Any<int>());
        }
    }
}