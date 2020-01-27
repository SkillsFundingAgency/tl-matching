using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_SubmitAction_CompleteProvisionGaps : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_SubmitAction_CompleteProvisionGaps(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.SaveSelectedOpportunitiesAsync(new ContinueOpportunityViewModel
            {
                SubmitAction = "CompleteProvisionGaps"
            }).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_Result_Is_Redirect_To_Start()
        {
            _result.Should().NotBeNull();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull(); 
            redirect?.RouteName.Should().Be("Start");
        }

        [Fact]
        public void Then_OpportunityService_GetOpportunityBasket_Is_Not_Called()
        {
            _fixture.OpportunityService.DidNotReceive().GetOpportunityBasketAsync(1);
        }

        [Fact]
        public void Then_ModelState_Is_Valid()
        {
            _fixture.Sut.ViewData.ModelState.IsValid.Should().BeTrue();
            _fixture.Sut.ViewData.ModelState.Count.Should().Be(0);
        }
    }
}