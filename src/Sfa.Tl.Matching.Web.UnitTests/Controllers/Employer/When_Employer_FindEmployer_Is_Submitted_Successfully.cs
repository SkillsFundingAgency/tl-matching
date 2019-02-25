using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
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
        private readonly EmployerNameViewModel _viewModel = new EmployerNameViewModel();

        private const int OpportunityId = 1;

        public When_Employer_FindEmployer_Is_Submitted_Successfully()
        {
            _viewModel.OpportunityId = OpportunityId;
            _viewModel.BusinessName = EmployerName;

            _employerService = Substitute.For<IEmployerService>();
            _employerService.GetEmployer(Arg.Any<string>(), Arg.Any<string>()).Returns(new EmployerDto());

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
            _employerService.Received(1).GetEmployer(EmployerName, "");
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(_dto);
        }

        [Fact]
        public void Then_EmployerName_Is_Populated()
        {
            _dto.EmployerName.Should().Be(EmployerName);
        }

        [Fact]
        public void Then_ModifiedBy_Is_Populated()
        {
            _dto.ModifiedBy.Should().Be(ModifiedBy);
        }
    }
}