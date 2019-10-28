using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.MatchingServiceProviderOpportunityReport
{
    public class When_MatchingServiceProviderOpportunityReport_Is_Called_For_Single_Provider_Multiple_Venues : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceProviderOpportunityReport> _result;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceProviderOpportunityReport_Is_Called_For_Single_Provider_Multiple_Venues()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);
            
            ClearData();
            
            var employer = _employerBuilder.CreaeEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreaeProvider(2);

            _savedReferralOpportunityItem = _opportunityBuilder.CreaeReferralOpportunityItem(true, true, provider.ProviderVenue.Select(pv => pv.Id).ToArray());

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem });

            _result = _dbContext.MatchingServiceProviderOpportunityReport.ToList();
        }

        [Fact]
        public void Then_ProviderVenue_Should_Have_One_OpportunityItem()
        {
            _result.Should().NotBeNull();
            var item1 = _result.SingleOrDefault(o => o.ProviderVenuePostCode == _savedReferralOpportunityItem.Referral.ElementAt(0).ProviderVenue.Postcode);
            item1.Should().NotBeNull();
            item1?.OpportunityItemCount.Should().Be(1);

            var item2 = _result.SingleOrDefault(o => o.ProviderVenuePostCode == _savedReferralOpportunityItem.Referral.ElementAt(1).ProviderVenue.Postcode);
            item2.Should().NotBeNull();
            item2?.OpportunityItemCount.Should().Be(1);
        }

        public void Dispose()
        {
            ClearData();

            _dbContext?.Dispose();
        }

        private void ClearData()
        {
            _opportunityBuilder.ClearData();
            _providerBuilder.ClearData();
            _employerBuilder.ClearData();
        }
    }
}