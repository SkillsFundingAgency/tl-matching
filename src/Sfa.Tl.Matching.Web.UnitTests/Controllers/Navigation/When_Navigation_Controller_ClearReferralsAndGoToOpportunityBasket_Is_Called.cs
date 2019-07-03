using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_ClearReferralsAndGoToOpportunityBasket_Is_Called
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Navigation_Controller_ClearReferralsAndGoToOpportunityBasket_Is_Called()
        {
            _opportunityService = Substitute.For<IOpportunityService>();

            var navigationController = new NavigationController(_opportunityService);

            _result = navigationController.ClearReferralsAndGoToOpportunityBasket(1, 2).GetAwaiter().GetResult();
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
        public void Then_ClearOpportunityItemsSelectedForReferralAsync_Is_Called_With_Expected_Field_Values()
        {
            _opportunityService
                .Received(1)
                .ClearOpportunityItemsSelectedForReferralAsync(Arg.Is<int>(
                    id => id == 1
                ));
        }
    }
}
