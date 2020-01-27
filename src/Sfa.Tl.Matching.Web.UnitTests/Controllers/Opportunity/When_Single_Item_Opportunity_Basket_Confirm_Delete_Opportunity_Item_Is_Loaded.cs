using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Single_Item_Opportunity_Basket_Confirm_Delete_Opportunity_Item_Is_Loaded : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_Single_Item_Opportunity_Basket_Confirm_Delete_Opportunity_Item_Is_Loaded(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;

            _fixture.OpportunityService.GetConfirmDeleteOpportunityItemAsync(_fixture.OpportunityId).Returns(new ConfirmDeleteOpportunityItemViewModel
            {
                OpportunityItemId = _fixture.OpportunityItemId,
                OpportunityId = _fixture.OpportunityId,
                CompanyName = "Company Name",
                CompanyNameAka = "Also Known As",
                Postcode = "PostCode",
                JobRole = "JobRole",
                BasketItemCount = 1,
                Placements = 1,
            });

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("CreatedBy");

            _result = controllerWithClaims.GetConfirmDeleteOpportunityItemAsync(_fixture.OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            var viewModel = _result.GetViewModel<ConfirmDeleteOpportunityItemViewModel>();
            viewModel.OpportunityItemId.Should().Be(_fixture.OpportunityItemId);
            viewModel.OpportunityId.Should().Be(_fixture.OpportunityId);
            viewModel.CompanyName.Should().Be("Company Name");
            viewModel.CompanyNameAka.Should().Be("Also Known As");
            viewModel.CompanyNameWithAka.Should().Be("Company Name (Also Known As)");
            viewModel.Postcode.Should().Be("PostCode");
            viewModel.JobRole.Should().Be("JobRole");
            viewModel.PlacementsDetail.Should().Be("At least 1");
            viewModel.DeleteWarningText.Should().Be("This cannot be undone and will mean this employer has no more saved opportunities.");
            viewModel.SubmitActionText.Should().Be("Confirm and finish");
        }

        [Fact]
        public void GetConfirmDeleteOpportunityItemAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _fixture.OpportunityService.Received(1).GetConfirmDeleteOpportunityItemAsync(1);
        }
    }
}