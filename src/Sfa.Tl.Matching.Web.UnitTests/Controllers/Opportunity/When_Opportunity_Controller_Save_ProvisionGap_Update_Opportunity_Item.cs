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
    public class When_Opportunity_Controller_Save_ProvisionGap_Update_Opportunity : IClassFixture<OpportunityControllerFixture<OpportunityDto, SaveProvisionGapViewModel>>
    {
        private readonly OpportunityControllerFixture<OpportunityDto, SaveProvisionGapViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_ProvisionGap_Update_Opportunity(OpportunityControllerFixture<OpportunityDto, SaveProvisionGapViewModel> fixture)
        {
            _fixture = fixture;
            
            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveProvisionGapAsync(new SaveProvisionGapViewModel
                    {SearchResultProviderCount = 0, SelectedRouteId = 1, Postcode = "cv12wt", SearchRadius = 10})
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Not_Called()
        {
            _fixture.OpportunityService
                .DidNotReceive()
                .CreateOpportunityAsync(Arg.Any<OpportunityDto>());
        }

        [Fact]
        public void Then_CreateOpportunityItem_Is_Not_Called()
        {
            _fixture.OpportunityService
                .DidNotReceive()
                .CreateOpportunityItemAsync(Arg.Any<OpportunityItemDto>());
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService
                .Received(2)
                .UpdateOpportunityItemAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetPlacementInformation()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetPlacementInformation");
        }
    }
}