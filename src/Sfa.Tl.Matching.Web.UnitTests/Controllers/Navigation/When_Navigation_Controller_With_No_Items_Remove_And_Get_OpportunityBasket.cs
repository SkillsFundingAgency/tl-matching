using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Filters;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_With_No_Items_Remove_And_Get_OpportunityBasket
    {
        private readonly IActionResult _result;

        public When_Navigation_Controller_With_No_Items_Remove_And_Get_OpportunityBasket()
        {
            var opportunityService = Substitute.For<IOpportunityService>();
            var backLinkService = Substitute.For<IBackLinkService>();
            var urlList = Substitute.For<NavigationManager>();

            opportunityService.GetSavedOpportunityItemCountAsync(Arg.Any<int>()).Returns(Task.FromResult(0));

            var navigationController = new NavigationController(opportunityService, backLinkService, urlList);

            _result = navigationController.RemoveOpportunityItemAndGetOpportunityBasket(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("Start");
            result?.RouteName.Should().NotBe("GetOpportunityBasket");
        }
    }
}
