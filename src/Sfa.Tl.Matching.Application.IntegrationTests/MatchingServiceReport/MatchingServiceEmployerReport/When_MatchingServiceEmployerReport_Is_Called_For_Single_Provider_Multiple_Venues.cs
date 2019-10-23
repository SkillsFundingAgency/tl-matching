using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.MatchingServiceEmployerReport
{
    public class When_MatchingServiceEmployerReport_Is_Called_For_Single_Provider_Multiple_Venues : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<MatchingServiceOpportunityReport> _result;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceEmployerReport_Is_Called_For_Single_Provider_Multiple_Venues()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            var employer = _employerBuilder.CreaeEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreaeProvider(2);

            _savedReferralOpportunityItem = _opportunityBuilder.CreaeReferralOpportunityItem(true, false, provider.ProviderVenue.Select(pv => pv.Id).ToArray());

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem });

            _result = _dbContext.MatchingServiceOpportunityReport.ToList();
        }

        [Fact]
        public void Then_Provider_Count_Should_Be_One()
        {
            _result.Should().NotBeNull();
            var item = _result.SingleOrDefault(o => o.OpportunityItemId == _savedReferralOpportunityItem.Id);
            item.Should().NotBeNull();
            item?.ProviderCount.Should().Be(1);
        }

        public void Dispose()
        {
            _opportunityBuilder.ClearData();
            _providerBuilder.ClearData();
            _employerBuilder.ClearData();

            _dbContext?.Dispose();
        }
    }
}