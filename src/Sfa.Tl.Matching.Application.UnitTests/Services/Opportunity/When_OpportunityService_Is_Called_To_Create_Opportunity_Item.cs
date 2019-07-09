using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity_Item
    {
        private readonly int _result;
        private const int OpportunityId = 101;
        private const int OpportunityItemId = 1;

        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IGoogleMapApiClient _googleMapApiClient;

        public When_OpportunityService_Is_Called_To_Create_Opportunity_Item()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "adminUserName")
                }))
            });

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<OpportunityItemDto, OpportunityItem>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<OpportunityItemDto, OpportunityItem>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityItemDto, OpportunityItem>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            
            _googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapApiClient.GetAddressDetails(Arg.Is<string>(s => s == "AA1 1AA")).Returns("Coventry");
            
            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityItemRepository.Create(Arg.Any<OpportunityItem>())
                .Returns(OpportunityItemId);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, provisionGapRepository, referralRepository, _googleMapApiClient);

            var dto = new OpportunityItemDto
            {
                OpportunityId = OpportunityId,
                OpportunityType = OpportunityType.Referral,
                RouteId = 5,
                Postcode = "AA1 1AA",
                SearchRadius = 10,
                JobRole = "Test Title",
                PlacementsKnown = true,
                Placements = 3,
                SearchResultProviderCount = 15,
                IsSaved = true,
                IsSelectedForReferral = true,
                IsCompleted = true
            };

            _result = opportunityService.CreateOpportunityItemAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityItemRepository_Create_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .Create(Arg.Is<OpportunityItem>(opportunityItem =>
                    opportunityItem.OpportunityId == OpportunityId &&
                    opportunityItem.OpportunityType == OpportunityType.Referral.ToString() &&
                    opportunityItem.RouteId == 5 &&
                    opportunityItem.Postcode == "AA1 1AA" &&
                    opportunityItem.Town == "Coventry" &&
                    opportunityItem.SearchRadius == 10 &&
                    opportunityItem.JobRole == "Test Title" &&
                    opportunityItem.PlacementsKnown.HasValue &&
                    opportunityItem.PlacementsKnown.Value &&
                    opportunityItem.Placements == 3 &&
                    opportunityItem.SearchResultProviderCount == 15 &&
                    opportunityItem.IsSaved &&
                    opportunityItem.IsSelectedForReferral &&
                    opportunityItem.IsCompleted &&
                    opportunityItem.CreatedBy == "adminUserName"
            ));
        }

        [Fact]
        public void Then_OpportunityItemId_Is_Created()
        {
            _result.Should().Be(OpportunityItemId);
        }

        [Fact]
        public void Then_GoogleMapApiClient_GetAddressDetails_Is_Called_Exactly_Once()
        {
            _googleMapApiClient.Received(1).GetAddressDetails(Arg.Is<string>(s => s == "AA1 1AA"));
        }
    }
}