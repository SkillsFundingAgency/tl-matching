using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully : IClassFixture<OpportunityControllerFixture<CheckAnswersDto, CheckAnswersViewModel>>
    {
        private readonly OpportunityControllerFixture<CheckAnswersDto, CheckAnswersViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Recording_Referrals_And_Check_Answers_Is_Submitted_Successfully(OpportunityControllerFixture<CheckAnswersDto, CheckAnswersViewModel> fixture)
        {
            _fixture = fixture;
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("ModifiedBy");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveCheckAnswers(_fixture.OpportunityId, _fixture.OpportunityItemId).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();
            result?.RouteName.Should().Be("GetOpportunityBasket");
            result?.RouteValues["opportunityId"].Should().Be(_fixture.OpportunityId);
            result?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }

        [Fact]
        public void Then_UpdateOpportunityItemAsync_Is_Called_Exactly_Once()
        {
            // TODO Assert args
            _fixture.OpportunityService.Received(2).UpdateOpportunityItemAsync(Arg.Any<CheckAnswersDto>());
        }
    }
}