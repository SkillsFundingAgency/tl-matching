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

namespace Sfa.Tl.Matching.Application.IntegrationTests.Opportunity
{
    public class OpportunityTestFixture : IDisposable
    {
        internal readonly IOpportunityService OpportunityService;
        internal MatchingDbContext MatchingDbContext;

        public OpportunityTestFixture()
        {
            var loggerRepository = new Logger<OpportunityRepository>(new NullLoggerFactory());
            var loggerProvisionGapRepository = new Logger<ProvisionGapRepository>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var opportunityRepository = new OpportunityRepository(loggerRepository, MatchingDbContext);
            var provisionGapRepository = new ProvisionGapRepository(loggerProvisionGapRepository, MatchingDbContext);
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var dateTimeProvider = new DateTimeProvider();

            OpportunityService = new OpportunityService(mapper, dateTimeProvider, opportunityRepository, provisionGapRepository);
        }

        internal void ResetData(string postcode)
        {
            var opportunity = MatchingDbContext.Opportunity.FirstOrDefault(o => o.PostCode == postcode);
            if (opportunity != null)
            {
                MatchingDbContext.Opportunity.Remove(opportunity);
                var count = MatchingDbContext.SaveChanges();
                count.Should().Be(1);
            }
        }

        internal int GetCountBy(string postcode)
        {
            var opportunityCount = MatchingDbContext.Opportunity.Count(o => o.PostCode == postcode);

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