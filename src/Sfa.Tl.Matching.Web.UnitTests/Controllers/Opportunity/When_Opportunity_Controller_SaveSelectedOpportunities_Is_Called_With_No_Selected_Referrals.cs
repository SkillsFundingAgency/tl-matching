using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_No_Selected_Referrals : IClassFixture<OpportunityControllerFixture>
    {
        private readonly OpportunityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_Opportunity_Controller_SaveSelectedOpportunities_Is_Called_With_No_Selected_Referrals(OpportunityControllerFixture fixture)
        {
            _fixture = fixture;
            
            _fixture.OpportunityService.GetOpportunityBasketAsync(_fixture.OpportunityId)
                .Returns(new OpportunityBasketViewModel());

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _result = controllerWithClaims.SaveSelectedOpportunitiesAsync(new ContinueOpportunityViewModel
            {
                OpportunityId = _fixture.OpportunityId,
                SubmitAction = "SaveSelectedOpportunities",
                SelectedOpportunity = new List<SelectedOpportunityItemViewModel>
                {
                    new SelectedOpportunityItemViewModel
                    {
                        IsSelected = false,
                        OpportunityType = OpportunityType.Referral.ToString()
                    }
                }
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityService_GetOpportunityBasket_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService.Received(2).GetOpportunityBasketAsync(_fixture.OpportunityId);
        }

        [Fact]
        public void Then_OpportunityService_ContinueWithOpportunities_Is_Not_Called()
        {
            _fixture.OpportunityService.DidNotReceive().ContinueWithOpportunitiesAsync(Arg.Any<ContinueOpportunityViewModel>());
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_ModelState_Is_Not_Valid()
        {
            _fixture.Sut.ViewData.ModelState.IsValid.Should().BeFalse();
            _fixture.Sut.ViewData.ModelState.Count.Should().Be(1);
            _fixture.Sut.ViewData.ModelState["ReferralItems[0].IsSelected"].Errors.Should().ContainSingle(error =>
                error.ErrorMessage == "You must select an opportunity to continue");
        }
    }
}