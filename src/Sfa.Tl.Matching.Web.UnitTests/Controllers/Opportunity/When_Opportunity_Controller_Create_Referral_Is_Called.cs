using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class When_Opportunity_Controller_Create_Referral_Is_Called
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IActionResult _result;
        private const string UserName = "username";
        private const string Email = "email@address.com";

        public When_Opportunity_Controller_Create_Referral_Is_Called()
        {
            const int opportunityId = 1;
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.CreateOpportunity(Arg.Any<OpportunityDto>()).Returns(opportunityId);

            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(EmployerDtoMapper).Assembly);
                c.ConstructServicesUsing(type =>
                {
                    if (type.FullName.Contains("LoggedInUserEmailResolver"))
                        return new LoggedInUserEmailResolver<CreateReferralViewModel, OpportunityDto>(httpcontextAccesor);
                    if (type.FullName.Contains("LoggedInUserNameResolver") && type.FullName.Contains("CreateReferralViewModel"))
                        return new LoggedInUserNameResolver<CreateReferralViewModel, OpportunityDto>(httpcontextAccesor);
                    if (type.FullName.Contains("LoggedInUserNameResolver") && type.FullName.Contains("SelectedProviderViewModel"))
                        return new LoggedInUserNameResolver<SelectedProviderViewModel, ReferralDto>(httpcontextAccesor);
                    //if (type.Name.Contains("UtcNowResolver"))
                    //    return new UtcNowResolver<CreateReferralViewModel, OpportunityDto>(new DateTimeProvider());

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

            _result = controllerWithClaims.CreateReferral(new CreateReferralViewModel
            {
                SearchResultProviderCount = 2,
                SelectedRouteId = 1,
                Postcode = "cv12wt",
                SearchRadius = 10,
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
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_CreateOpportunity_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).CreateOpportunity(Arg.Is<OpportunityDto>(arg =>
                arg.SearchResultProviderCount == 2 &&
                arg.RouteId == 1 &&
                arg.Postcode == "cv12wt" &&
                arg.SearchRadius == 10 &&
                arg.Referral.Count == 1 &&
                arg.Referral.ElementAt(0).ProviderVenueId == 2 &&
                arg.Referral.ElementAt(0).DistanceFromEmployer == 3.4m
                ));
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            var result = _result as RedirectToRouteResult;

            result.Should().NotBeNull();

            result?.RouteName.Should().Be("PlacementInformationSave_Get");
        }
    }
}