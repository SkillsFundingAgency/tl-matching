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
    public class When_Opportunity_Basket_Is_Loaded : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_Opportunity_Basket_Is_Loaded( OpportunityControllerFixture fixture)
        {
            _fixture = fixture;

            _fixture.OpportunityService.GetOpportunityBasketAsync(_fixture.OpportunityId).Returns(new OpportunityBasketViewModel
            {
                OpportunityId = _fixture.OpportunityId,
                CompanyName = _fixture.CompanyName,
                CompanyNameAka = _fixture.CompanyNameAka
            });

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("CreatedBy");

            _result = controllerWithClaims.GetOpportunityBasketAsync(_fixture.OpportunityId, _fixture.OpportunityItemId)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
            var viewModel = _result.GetViewModel<OpportunityBasketViewModel>();
            viewModel.CompanyName.Should().Be(_fixture.CompanyName);
            viewModel.CompanyNameAka.Should().Be(_fixture.CompanyNameAka);
            viewModel.CompanyNameWithAka.Should().Be($"{_fixture.CompanyName} ({_fixture.CompanyNameAka})");
        }

        [Fact]
        public void Clear_And_Load_Is_Called_Exactly_Once_In_Correct_Order()
        {
            Received.InOrder(() =>
                {
                    _fixture.OpportunityService.Received(1).ClearOpportunityItemsSelectedForReferralAsync(_fixture.OpportunityId);
                    _fixture.OpportunityService.Received(1).GetOpportunityBasketAsync(_fixture.OpportunityId);

                });
        }
    }
}