using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
    public class When_Employer_Confirm_Remove_Employer
    {
        private IActionResult _result;

        private const int OpportunityId = 12;
        private const string CompanyName = "CompanyName";
        private readonly IEmployerService _employerService;
        private readonly EmployerController _employerController;

        public When_Employer_Confirm_Remove_Employer()
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
        public void Then_Result_Is_Not_Null()
        {
            _employerService.GetConfirmDeleteEmployerOpportunity(Arg.Any<int>(), Arg.Any<string>()).Returns(new RemoveEmployerDto
            {
                EmployerName = CompanyName,
                OpportunityCount = 10,
                EmployerCount = 20
            });

            _result = _employerController.ConfirmDelete(OpportunityId).GetAwaiter().GetResult();

            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_View_Result_Is_Returned()
        {
            _employerService.GetConfirmDeleteEmployerOpportunity(Arg.Any<int>(), Arg.Any<string>()).Returns(new RemoveEmployerDto
            {
                EmployerName = CompanyName,
                OpportunityCount = 10,
                EmployerCount = 20
            });

            _result = _employerController.ConfirmDelete(OpportunityId).GetAwaiter().GetResult();

            var viewModel = _result as ViewResult;
            viewModel.Should().NotBeNull();
        }

        [Fact]
        public void Then_Confirm_Remove_Employer_Model_Is_Loaded()
        {
            _employerService.GetConfirmDeleteEmployerOpportunity(Arg.Any<int>(), Arg.Any<string>()).Returns(new RemoveEmployerDto
            {
                EmployerName = CompanyName,
                OpportunityCount = 10,
                EmployerCount = 20
            });

            _result = _employerController.ConfirmDelete(OpportunityId).GetAwaiter().GetResult();

            var viewModel = _result.GetViewModel<RemoveEmployerViewModel>();

            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.ConfirmDeleteText.Should()
                .Be($"Confirm you want to delete {10} opportunities created for {CompanyName}");
            viewModel.WarningDeleteText.Should().Be("This cannot be undone.");
            viewModel.EmployerCount.Should().Be(20);
        }

        [Fact]
        public void Then_Confirm_Remove_Employer_Model_Is_Loaded_With_No_Employer()
        {
            _employerService.GetConfirmDeleteEmployerOpportunity(Arg.Any<int>(), Arg.Any<string>()).Returns(new RemoveEmployerDto
            {
                EmployerName = CompanyName,
                OpportunityCount = 1,
                EmployerCount = 1
            });

            _result = _employerController.ConfirmDelete(OpportunityId).GetAwaiter().GetResult();

            var viewModel = _result.GetViewModel<RemoveEmployerViewModel>();

            viewModel.OpportunityId.Should().Be(OpportunityId);
            viewModel.ConfirmDeleteText.Should()
                .Be($"Confirm you want to delete {1} opportunity created for {CompanyName}");
            viewModel.WarningDeleteText.Should().Be("This cannot be undone and will mean you have no more employers with saved opportunities.");
            viewModel.SubmitActionText.Should().Be("Confirm and finish");
            viewModel.EmployerCount.Should().Be(1);
        }
    }
}