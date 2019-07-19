using System;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Opportunity
{
    public class OpportunityTestFixture : IDisposable
    {
        internal readonly IOpportunityService OpportunityService;
        internal MatchingDbContext MatchingDbContext;

        public OpportunityTestFixture()
        {
            var loggerRepository = new Logger<OpportunityRepository>(new NullLoggerFactory());
            var loggerOpportunityItemRepository = new Logger<GenericRepository<OpportunityItem>>(new NullLoggerFactory());
            var loggerProvisionGapRepository = new Logger<GenericRepository<ProvisionGap>>(new NullLoggerFactory());
            var loggerReferralRepository = new Logger<GenericRepository<Referral>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var opportunityRepository = new OpportunityRepository(loggerRepository, MatchingDbContext);
            var opportunityItemRepository = new GenericRepository<OpportunityItem>(loggerOpportunityItemRepository, MatchingDbContext);
            var provisionGapRepository = new GenericRepository<ProvisionGap>(loggerProvisionGapRepository, MatchingDbContext);
            var referralRepository = new GenericRepository<Referral>(loggerReferralRepository, MatchingDbContext);
            
            var googleMapsApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

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
                        new LoggedInUserEmailResolver<OpportunityDto, Domain.Models.Opportunity>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<OpportunityDto, Domain.Models.Opportunity>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityDto, Domain.Models.Opportunity>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            OpportunityService = new OpportunityService(
                mapper, 
                opportunityRepository, 
                opportunityItemRepository, 
                provisionGapRepository, 
                referralRepository,
                googleMapsApiClient,
                opportunityPipelineReportWriter,
                dateTimeProvider);
        }

        internal void ResetData(string employerContact)
        {
            var opportunity = MatchingDbContext.Opportunity.FirstOrDefault(o => o.EmployerContact == employerContact);
            if (opportunity != null)
            {
                MatchingDbContext.Opportunity.Remove(opportunity);
                var count = MatchingDbContext.SaveChanges();
                count.Should().Be(1);
            }
        }

        internal int GetCountBy(string employerContact)
        {
            var opportunityCount = MatchingDbContext.Opportunity.Count(o => o.EmployerContact == employerContact);

            return opportunityCount;
        }

        internal Domain.Models.Opportunity Get(int id)
        {
            var opportunity = MatchingDbContext.Opportunity.FirstOrDefault(o => o.Id == id);

            return opportunity;
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}