using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_With_Items_Get_Placement_Or_Employer
    {
        private readonly IActionResult _result;

        public When_Navigation_Controller_With_Items_Get_Placement_Or_Employer()
        {
            var opportunityService = Substitute.For<IOpportunityService>();
            opportunityService.GetOpportunityItemCountAsync(Arg.Any<int>()).Returns(Task.FromResult(1));

            var navigationController = new NavigationController(opportunityService);

            _result = navigationController.GetPlacementOrEmployer(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetOpportunityBasket()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetEmployerDetails");
            result?.RouteName.Should().NotBe("GetPlacementInformation");
            result?.RouteValues["opportunityId"].Should().Be(1);
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}
