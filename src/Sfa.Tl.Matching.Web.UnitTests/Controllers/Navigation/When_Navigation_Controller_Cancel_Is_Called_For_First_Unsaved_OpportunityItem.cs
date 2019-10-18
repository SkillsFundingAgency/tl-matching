using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_Cancel_Is_Called_For_First_Unsaved_OpportunityItem
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_Navigation_Controller_Cancel_Is_Called_For_First_Unsaved_OpportunityItem()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            var backLinkService = Substitute.For<INavigationService>();
            
            _opportunityService.GetSavedOpportunityItemCountAsync(Arg.Any<int>()).Returns(Task.FromResult(0));

            var navigationController = new NavigationController(_opportunityService, backLinkService);

            _result = navigationController.RemoveAndGetOpportunityBasketAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("Start");
        }

        [Fact]
        public void Then_DeleteOpportunityItem_should_Be_Called_exeactly_Once()
        {
            _opportunityService.Received(1).DeleteOpportunityItemAsync(Arg.Any<int>(), Arg.Any<int>());
        }
    }
}
