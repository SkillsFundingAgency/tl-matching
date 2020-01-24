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
    public class When_Opportunity_Controller_Save_ProvisionGap_New_Opportunity : IClassFixture<OpportunityControllerFixture<OpportunityDto, SaveProvisionGapViewModel>>
    {
        private readonly OpportunityControllerFixture<OpportunityDto, SaveProvisionGapViewModel> _fixture;
        private const string UserName = "username";
        

        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_ProvisionGap_New_Opportunity(OpportunityControllerFixture<OpportunityDto, SaveProvisionGapViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.OpportunityService
                .IsNewProvisionGapAsync(Arg.Any<int>())
                .Returns(true);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveProvisionGapAsync(new SaveProvisionGapViewModel
            {
                OpportunityId = 0,
                OpportunityItemId = 0,
                SearchResultProviderCount = 0,
                SelectedRouteId = 1,
                Postcode = "cv12wt",
                SearchRadius = 10
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Not_Called()
        {
            _fixture.OpportunityService.DidNotReceive().UpdateOpportunityAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Not_Called()
        {
            _fixture.OpportunityService
                .DidNotReceive()
                .UpdateOpportunityItemAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService
                .Received(1)
                .CreateOpportunityAsync(Arg.Any<OpportunityDto>());
        }
        
        [Fact]
        public void Then_CreateOpportunityItem_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService
                .Received(5)
                .CreateOpportunityItemAsync(Arg.Any<OpportunityItemDto>());
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