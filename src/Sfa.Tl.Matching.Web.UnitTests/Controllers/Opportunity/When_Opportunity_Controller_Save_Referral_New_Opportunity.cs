using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Opportunity_Controller_Save_Referral_New_Opportunity
    {
        private readonly IOpportunityService _opportunityService;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_Referral_New_Opportunity()
        {
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService
                .IsNewReferralAsync(Arg.Any<int>())
                .Returns(true);

            _opportunityService.CreateOpportunityItemAsync(Arg.Any<OpportunityItemDto>()).Returns(2);

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                {
                    if (type.FullName.Contains("LoggedInUserEmailResolver"))
                        return new LoggedInUserEmailResolver<SaveReferralViewModel, OpportunityDto>(httpcontextAccesor);
                    if (type.FullName.Contains("LoggedInUserNameResolver") && type.FullName.Contains("SaveReferralViewModel"))
                        return new LoggedInUserNameResolver<SaveReferralViewModel, OpportunityDto>(httpcontextAccesor);
                    if (type.FullName.Contains("LoggedInUserNameResolver") && type.FullName.Contains("SelectedProviderViewModel"))
                        return new LoggedInUserNameResolver<SelectedProviderViewModel, ReferralDto>(httpcontextAccesor);

                    return null;
                });
            });
            var mapper = new Mapper(config);

            var opportunityController = new OpportunityController(_opportunityService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(UserName)
                .AddEmail(Email)
                .Build();

            httpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            var viewModel = new SaveReferralViewModel
            {
                SearchResultProviderCount = 2,
                SelectedRouteId = 1,
                Postcode = "cv12wt",
                OpportunityId = 0,
                OpportunityItemId = 0,
                SelectedProvider = new[]
                {
                    new SelectedProviderViewModel
                    {
                        ProviderVenueId = 1,
                        DistanceFromEmployer = 1.2m,
                        JourneyTimeByCar = 600,
                        JourneyTimeByPublicTransport = 900,
                        IsSelected = false
                    },
                    new SelectedProviderViewModel
                    {
                        ProviderVenueId = 2,
                        DistanceFromEmployer = 3.4m,
                        JourneyTimeByCar = 1200,
                        JourneyTimeByPublicTransport = 1800,
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
        public void Then_UpdateOpportunity_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceive()
                .UpdateOpportunityAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_UpdateOpportunityItem_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceive()
                .UpdateOpportunityItemAsync(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .CreateOpportunityAsync(Arg.Any<OpportunityDto>());
        }

        [Fact]
        public void Then_CreateOpportunityItem_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .CreateOpportunityItemAsync(Arg.Any<OpportunityItemDto>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_to_GetPlacementInformation()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetPlacementInformation");
            result?.RouteValues["opportunityItemId"].Should().Be(2);
        }
    }
}