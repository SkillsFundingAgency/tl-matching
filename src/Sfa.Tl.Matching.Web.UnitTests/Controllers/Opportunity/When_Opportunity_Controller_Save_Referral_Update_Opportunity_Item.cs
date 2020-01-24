using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Save_Referral_Update_Opportunity : IClassFixture<OpportunityControllerFixture<OpportunityDto, SaveReferralViewModel>>
    {
        private readonly OpportunityControllerFixture<OpportunityDto, SaveReferralViewModel> _fixture;
        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_Referral_Update_Opportunity(OpportunityControllerFixture<OpportunityDto, SaveReferralViewModel> fixture)
        {
            _fixture = fixture;

            _fixture.OpportunityService.IsNewReferralAsync(_fixture.OpportunityItemId).Returns(false);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims("username");

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            var viewModel = new SaveReferralViewModel
            {
                SearchResultProviderCount = 2,
                SelectedRouteId = 1,
                Postcode = "cv12wt",
                SearchRadius = 10,
                OpportunityId = _fixture.OpportunityId,
                OpportunityItemId = _fixture.OpportunityItemId,
                SelectedProvider = new[]
                {
                    new SelectedProviderViewModel
                    {
                        ProviderVenueId = 1,
                        DistanceFromEmployer = 1.2m,
                        IsSelected = false
                    },
                    new SelectedProviderViewModel
                    {
                        ProviderVenueId = 2,
                        DistanceFromEmployer = 3.4m,
                        IsSelected = true
                    }
                }
            };

            var serializeObject = JsonConvert.SerializeObject(viewModel);

            var tempDataProvider = Substitute.For<ITempDataProvider>();
            var tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            var tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());
            tempData.Add("SelectedProviders", serializeObject);
            controllerWithClaims.TempData = tempData;

            _result = controllerWithClaims.SaveReferralAsync().GetAwaiter().GetResult();
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
                .Received(4)
                .UpdateOpportunityItemAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_GetPlacementInformation()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetPlacementInformation");
            result?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }
    }
}