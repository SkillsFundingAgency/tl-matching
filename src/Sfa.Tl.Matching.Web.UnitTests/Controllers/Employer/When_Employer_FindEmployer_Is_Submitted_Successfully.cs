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
        private const string EmployerName = "EmployerName";
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

            var tempData = Substitute.For<ITempDataDictionary>();
            var employerController = new EmployerController(_employerService, _opportunityService);
            employerController.AddUsernameToContext(ModifiedBy);

            employerController.TempData = tempData;
            employerController.FindEmployer(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerService.Received(1).GetEmployer(EmployerId);
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(Arg.Any<OpportunityDto>());
        }
    }
}