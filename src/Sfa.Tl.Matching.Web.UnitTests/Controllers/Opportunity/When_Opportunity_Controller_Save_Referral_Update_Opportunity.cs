using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class When_Opportunity_Controller_Save_Referral_Update_Opportunity
    {
        private readonly IOpportunityService _opportunityService;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        private readonly IActionResult _result;

        public When_Opportunity_Controller_Save_Referral_Update_Opportunity()
        {
            const int opportunityId = 1;
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.IsNewReferral(opportunityId).Returns(false);

            var referralService = Substitute.For<IReferralService>();

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(EmployerDtoMapper).Assembly);
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

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
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
                SearchRadius = 10,
                OpportunityId = opportunityId,
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

            _result = controllerWithClaims.SaveReferral(serializeObject).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Not_Called()
        {
            _opportunityService.DidNotReceive().CreateOpportunity(Arg.Any<OpportunityDto>());
        }

        [Fact]
        public void Then_UpdateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).UpdateOpportunity(Arg.Any<ProviderSearchDto>());
        }

        [Fact]
        public void Then_Result_Is_Redirect_to_PlacementInformationSave_Get()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("PlacementInformationSave_Get");
        }
    }
}