using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Details_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        private const int OpportunityId = 12;
        private const string EmployerName = "CompanyName";

        public When_Employer_Details_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var employerService = Substitute.For<IEmployerService>();
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(new OpportunityDto
            {
                Id = OpportunityId,
                EmployerName = EmployerName
            });

            var employerController = new EmployerController(employerService, _opportunityService, mapper);

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
            viewModel.CompanyName.Should().Be(EmployerName);
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }
    }
}