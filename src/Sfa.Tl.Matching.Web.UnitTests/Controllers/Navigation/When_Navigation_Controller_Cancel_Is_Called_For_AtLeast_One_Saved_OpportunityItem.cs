using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_Cancel_Is_Called_For_AtLeast_One_Saved_OpportunityItem
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_Navigation_Controller_Cancel_Is_Called_For_AtLeast_One_Saved_OpportunityItem()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            var backLinkService = Substitute.For<INavigationService>();
            
            _opportunityService.GetSavedOpportunityItemCountAsync(Arg.Any<int>()).Returns(Task.FromResult(1));

            var navigationController = new NavigationController(_opportunityService, backLinkService);

            _result = navigationController.RemoveAndGetOpportunityBasketAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetOpportunityBasket");
            result?.RouteValues["opportunityId"].Should().Be(1);
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }

        [Fact]
        public void Then_DeleteOpportunityItem_should_NOT_Be_Called()
        {
            _opportunityService.DidNotReceive().DeleteOpportunityItemAsync(Arg.Any<int>(), Arg.Any<int>());
        }
    }
}
