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
    public class When_MultiItem_Opportunity_Basket_Confirm_Delete_Opportunity_Item_Is_Loaded : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_MultiItem_Opportunity_Basket_Confirm_Delete_Opportunity_Item_Is_Loaded(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            _fixture.OpportunityService.GetConfirmDeleteOpportunityItemAsync(1).Returns(new ConfirmDeleteOpportunityItemViewModel
            {
                OpportunityItemId = _fixture.OpportunityItemId,
                OpportunityId = _fixture.OpportunityId,
                CompanyName = _fixture.CompanyName,
                CompanyNameAka = _fixture.CompanyNameAka,
                Postcode = _fixture.Postcode,
                JobRole = _fixture.JobRole,
                BasketItemCount = _fixture.BasketItemCount,
            });

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("CreatedBy");

            _result = controllerWithClaims.GetConfirmDeleteOpportunityItemAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            var viewModel = _result.GetViewModel<ConfirmDeleteOpportunityItemViewModel>();
            viewModel.OpportunityItemId.Should().Be(1);
            viewModel.OpportunityId.Should().Be(2);
            viewModel.CompanyName.Should().Be("Company Name1");
            viewModel.CompanyNameAka.Should().Be("Also Known As 1");
            viewModel.CompanyNameWithAka.Should().Be("Company Name1 (Also Known As 1)");
            viewModel.Postcode.Should().Be("PostCode1");
            viewModel.JobRole.Should().Be("JobRole1");
            viewModel.PlacementsDetail.Should().Be("At least 1");
            viewModel.DeleteWarningText.Should().Be("This cannot be undone.");
            viewModel.SubmitActionText.Should().Be("Confirm and continue");
        }

        [Fact]
        public void GetConfirmDeleteOpportunityItemAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _fixture.OpportunityService.Received(1).GetConfirmDeleteOpportunityItemAsync(1);
        }
    }
}