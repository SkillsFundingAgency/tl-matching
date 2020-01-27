using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Single_Item_Opportunity_Basket_Delete_Opportunity_Item_Is_Posted : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_Single_Item_Opportunity_Basket_Delete_Opportunity_Item_Is_Posted(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("CreatedBy");

            _result = controllerWithClaims.DeleteOpportunityItemAsync(new DeleteOpportunityItemViewModel
            {
                OpportunityId = _fixture.OpportunityId, OpportunityItemId = _fixture.OpportunityItemId,
                BasketItemCount = 1
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Correct()
        {
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            _result.Should().NotBeNull();

            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("Start");
        }

        [Fact]
        public void DeleteOpportunityItemAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _fixture.OpportunityService.Received(2).DeleteOpportunityItemAsync(_fixture.OpportunityId, _fixture.OpportunityItemId);
        }
    }
}