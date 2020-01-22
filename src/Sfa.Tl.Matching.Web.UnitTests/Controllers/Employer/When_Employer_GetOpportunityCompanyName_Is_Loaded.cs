using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_GetOpportunityCompanyName_Is_Loaded : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;
        private readonly IActionResult _result;
        
        public When_Employer_GetOpportunityCompanyName_Is_Loaded(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.EmployerService.GetOpportunityEmployerAsync(_fixture.OpportunityId, _fixture.OpportunityItemId)
                .Returns(new FindEmployerViewModel
                {
                    OpportunityId = _fixture.OpportunityId,
                    OpportunityItemId = _fixture.OpportunityItemId,
                    SelectedEmployerCrmId = _fixture.EmployerCrmId,
                    CompanyName = _fixture.CompanyName
                });

            _result = _fixture.Sut.GetOpportunityCompanyNameAsync(_fixture.OpportunityId, _fixture.OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunityEmployer_Is_Called_Exactly_Once()
        {
            _fixture.EmployerService
                .Received(1)
                .GetOpportunityEmployerAsync(_fixture.OpportunityId, _fixture.OpportunityItemId);
        }

        [Fact]
        public void Then_FindEmployerViewModel_Has_All_Data_Item_Set_Correctly()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<FindEmployerViewModel>();
            viewModel.OpportunityItemId.Should().Be(_fixture.OpportunityItemId);
            viewModel.OpportunityId.Should().Be(_fixture.OpportunityId);
            viewModel.SelectedEmployerCrmId.Should().Be(_fixture.EmployerCrmId);
            viewModel.CompanyName.Should().Be(_fixture.CompanyName);
        }
    }
}