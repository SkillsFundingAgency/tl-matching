using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity_Item
    {
        private readonly int _result;
        private const int OpportunityId = 101;
        private const int OpportunityItemId = 1;

        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

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

            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityItemRepository.Create(Arg.Any<OpportunityItem>())
                .Returns(OpportunityItemId);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, provisionGapRepository, referralRepository);

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
                .Create(Arg.Is<OpportunityItem>(opportunity =>
                    opportunity.OpportunityId == OpportunityId &&
                    opportunity.OpportunityType == OpportunityType.Referral.ToString() &&
                    opportunity.RouteId == 5 &&
                    opportunity.Postcode == "AA1 1AA" &&
                    opportunity.SearchRadius == 10 &&
                    opportunity.JobRole == "Test Title" &&
                    opportunity.PlacementsKnown.HasValue &&
                    opportunity.PlacementsKnown.Value &&
                    opportunity.Placements == 3 &&
                    opportunity.SearchResultProviderCount == 15 &&
                    opportunity.IsSaved.HasValue &&
                    opportunity.IsSaved.Value &&
                    opportunity.IsSelectedForReferral.HasValue &&
                    opportunity.IsSelectedForReferral.Value &&
                    opportunity.IsCompleted.HasValue &&
                    opportunity.IsCompleted.Value &&
                    opportunity.CreatedBy == "adminUserName"
            ));
        }

        [Fact]
        public void Then_OpportunityItemId_Is_Created()
        {
            _result.Should().Be(OpportunityItemId);
        }
    }
}