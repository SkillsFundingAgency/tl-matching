﻿using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Update_Referrals
    {
        private readonly IRepository<Domain.Models.Referral> _referralRepository;

        public When_OpportunityService_Is_Called_To_Update_Referrals()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
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
                        new LoggedInUserEmailResolver<ReferralDto, Domain.Models.Referral>(httpContextAccessor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            new LoggedInUserNameResolver<ReferralDto, Domain.Models.Referral>(httpContextAccessor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ReferralDto, Domain.Models.Referral>(new DateTimeProvider()) :
                                null);
            });

            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            var opportunityService = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository, 
                provisionGapRepository, _referralRepository, googleMapApiClient, 
                opportunityPipelineReportWriter, dateTimeProvider);

            var dto = new OpportunityItemDto
            {
                Referral = new List<ReferralDto>
                {
                    new()
                    {
                        ProviderVenueId = 1,
                        DistanceFromEmployer = 3.5M
                    }
                }
            };

            opportunityService.UpdateReferralsAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_DeleteMany_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1)
                .DeleteManyAsync(Arg.Any<IList<Domain.Models.Referral>>());
        }

        [Fact]
        public void Then_CreateMany_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1)
                .CreateManyAsync(Arg.Any<IList<Domain.Models.Referral>>());
        }

        [Fact]
        public void Then_UpdateMany_Is_Called_Exactly_Once()
        {
            _referralRepository.Received(1)
                .UpdateManyAsync(Arg.Any<IList<Domain.Models.Referral>>());
        }
    }
}