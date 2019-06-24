using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Opportunity
{
    public class OpportunityTestFixture : IDisposable
    {
        internal readonly IOpportunityService OpportunityService;
        internal MatchingDbContext MatchingDbContext;

        public OpportunityTestFixture()
        {
            var loggerRepository = new Logger<GenericRepository<Domain.Models.Opportunity>>(new NullLoggerFactory());
            var loggerOpportunityItemRepository = new Logger<GenericRepository<Domain.Models.OpportunityItem>>(new NullLoggerFactory());
            var loggerProvisionGapRepository = new Logger<GenericRepository<ProvisionGap>>(new NullLoggerFactory());
            var loggerReferralRepository = new Logger<GenericRepository<Referral>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var opportunityRepository = new GenericRepository<Domain.Models.Opportunity>(loggerRepository, MatchingDbContext);
            var opportunityItemRepository = new GenericRepository<Domain.Models.OpportunityItem>(loggerOpportunityItemRepository, MatchingDbContext);
            var provisionGapRepository = new GenericRepository<ProvisionGap>(loggerProvisionGapRepository, MatchingDbContext);
            var referralRepository = new GenericRepository<Referral>(loggerReferralRepository, MatchingDbContext);

            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            OpportunityService = new OpportunityService(mapper, 
                opportunityRepository, opportunityItemRepository, 
                provisionGapRepository, referralRepository);
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