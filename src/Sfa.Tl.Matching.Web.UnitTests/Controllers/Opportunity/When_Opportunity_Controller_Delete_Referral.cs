using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Delete_Referral : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Opportunity_Controller_Delete_Referral(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("CreatedBy");

            _result = controllerWithClaims.DeleteReferralAsync(_fixture.ReferralIdToDelete, _fixture.OpportunityItemId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Delete_Referral_Result_Is_Correct()
        {
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
            _result.Should().NotBeNull();

            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetCheckAnswers");
            result?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }

        [Fact]
        public void Then_DeleteReferralAsync_Is_Called_Exactly_Once_In_Correct_Order()
        {
            _fixture.OpportunityService.Received(1).DeleteReferralAsync(_fixture.ReferralIdToDelete);
        }
    }
}
