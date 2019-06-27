using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_Details_Is_Loaded
    {
        private readonly IActionResult _result;

        private const int OpportunityId = 12;
        private const int OpportunityItemId = 34;
        private const string EmployerName = "EmployerName";
        private const string EmployerContact = "EmployerContact";
        private const string EmployerContactPhone = "EmployerContactPhone";
        private const string EmployerContactEmail = "EmployerContactEmail";

        public When_Employer_Details_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var employerService = Substitute.For<IEmployerService>();
            employerService.GetOpportunityEmployerDetailAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(new EmployerDetailsViewModel
            {
                OpportunityId = OpportunityId,
                EmployerName = EmployerName,
                EmployerContact = EmployerContact,
                EmployerContactPhone = EmployerContactPhone,
                EmployerContactEmail = EmployerContactEmail
            });

            var employerController = new EmployerController(employerService, Substitute.For<IOpportunityService>(), mapper);

            _result = employerController.GetOpportunityEmployerDetails(OpportunityId, OpportunityItemId).GetAwaiter().GetResult();
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
        public void Then_EmployerDetailsViewModel_Is_Populated_Correctly()
        {
            var viewModel = _result.GetViewModel<EmployerDetailsViewModel>();

            viewModel.OpportunityItemId.Should().Be(OpportunityItemId);
            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.EmployerName.Should().Be(EmployerName);
            viewModel.EmployerContact.Should().Be(EmployerContact);
            viewModel.EmployerContactPhone.Should().Be(EmployerContactPhone);
            viewModel.EmployerContactEmail.Should().Be(EmployerContactEmail);
        }

    }
}