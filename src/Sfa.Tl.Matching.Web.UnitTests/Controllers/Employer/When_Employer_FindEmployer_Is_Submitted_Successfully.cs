using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_FindEmployer_Is_Submitted_Successfully
    {
        private readonly IEmployerService _employerService;
        private readonly IOpportunityService _opportunityService;
        private readonly OpportunityDto _dto = new OpportunityDto();
        private const string EmployerName = "EmployerName";
        private const string UserEmail = "UserEmail";
        private const string ModifiedBy = "ModifiedBy";
        private readonly FindEmployerViewModel _viewModel = new FindEmployerViewModel();

        private const int OpportunityId = 1;
        private const int EmployerId = 2;

        public When_Employer_FindEmployer_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;
            _viewModel.BusinessName = EmployerName;
            _viewModel.SelectedEmployerId = 2;

            _employerService = Substitute.For<IEmployerService>();
            _employerService.GetEmployer(EmployerId).Returns(new ValidEmployerDtoBuilder().Build());

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunity(OpportunityId).Returns(_dto);

            var tempData = Substitute.For<ITempDataDictionary>();
            var employerController = new EmployerController(_employerService, _opportunityService);
            employerController.AddUsernameToContext(ModifiedBy);

            employerController.TempData = tempData;
            employerController.FindEmployer(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetOpportunity(OpportunityId);
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).GetEmployer(EmployerId);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(_dto);
        }

        [Fact]
        public void Then_EmployerCrmId_Is_Populated()
        {
            _dto.EmployerCrmId.Should().Be("D7A48843-44CA-46A4-A391-70D7B01C68BC");
        }

        [Fact]
        public void Then_EmployerName_Is_Populated()
        {
            _dto.EmployerName.Should().Be("EmployerName");
        }

        [Fact]
        public void Then_EmployerAupa_Is_Populated()
        {
            _dto.EmployerAupa.Should().Be("EmployerAupa");
        }

        [Fact]
        public void Then_EmployerOwner_Is_Populated()
        {
            _dto.EmployerOwner.Should().Be("EmployerOwner");
        }

        [Fact]
        public void Then_Contact_Is_Populated()
        {
            _dto.Contact.Should().Be("Contact");
        }

        [Fact]
        public void Then_ContactEmail_Is_Populated()
        {
            _dto.ContactEmail.Should().Be("ContactEmail");
        }

        [Fact]
        public void Then_ContactPhone_Is_Populated()
        {
            _dto.ContactPhone.Should().Be("ContactPhone");
        }

        [Fact]
        public void Then_ModifiedBy_Is_Populated()
        {
            _dto.ModifiedBy.Should().Be("ModifiedBy");
        }
    }
}