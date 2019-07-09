using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Clear_Opportunity_Items_Selected_For_Referral
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        private const int OpportunityId = 1;

        public When_OpportunityService_Is_Called_To_Clear_Opportunity_Items_Selected_For_Referral()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "TestUser")
                }))
            });

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<OpportunityItemIsSelectedForReferralDto, OpportunityItem>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<OpportunityItemIsSelectedForReferralDto, OpportunityItem>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityItemIsSelectedForReferralDto, OpportunityItem>(dateTimeProvider) :
                                null);
            });
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository
                .GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(new OpportunityItemListBuilder().Build().AsQueryable());

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, provisionGapRepository, referralRepository, googleMapApiClient);

            opportunityService.ClearOpportunityItemsSelectedForReferralAsync(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetMany_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>());
        }

        [Fact]
        public void Then_UpdateMany_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnly(Arg.Any<IList<OpportunityItem>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>()
                );
        }

        [Fact]
        public void Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Two_Items_With_Expected_Values()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnly(Arg.Is<IList<OpportunityItem>>(
                    o => o.Count == 2
                         && o.All(x => x.IsSelectedForReferral == false)
                         && o.All(x => x.ModifiedBy == "TestUser")
                         && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                         ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }
    }
}