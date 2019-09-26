using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Navigation
{
    public class When_Navigation_Controller_SaveEmployerOpportunity_Is_Called
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;

        public When_Navigation_Controller_SaveEmployerOpportunity_Is_Called()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            var backLinkService = Substitute.For<INavigationService>();
            
            var navigationController = new NavigationController(_opportunityService,backLinkService);

            _result = navigationController.SaveEmployerOpportunity(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetSavedEmployerOpportunity()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetSavedEmployerOpportunity");
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
