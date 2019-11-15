using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.MatchingServiceOpportunityReport
{
    public class When_MatchingServiceOpportunityReport_Is_Called_For_Saved_OpportunityOpportunities : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceOpportunityReport> _result;
        private readonly OpportunityItem _savedProvisionGapOpportunityItem;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityItem _unsavedProvisionGapOpportunityItem;
        private readonly OpportunityItem _unsavedReferralOpportunityItem;

        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceOpportunityReport_Is_Called_For_Saved_OpportunityOpportunities()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            ClearData();

            var employer = _employerBuilder.CreateEmployer(Guid.NewGuid());
            var provider1 = _providerBuilder.CreateProvider();
            var provider2 = _providerBuilder.CreateProvider();

            _savedReferralOpportunityItem = _opportunityBuilder.CreateReferralOpportunityItem(true, true, provider1.ProviderVenue.First().Id);
            _savedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(true, true);

            _unsavedReferralOpportunityItem = _opportunityBuilder.CreateReferralOpportunityItem(false, false, provider2.ProviderVenue.First().Id);
            _unsavedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(false, false);

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem, _savedProvisionGapOpportunityItem, _unsavedReferralOpportunityItem, _unsavedProvisionGapOpportunityItem });

            _result = _dbContext.MatchingServiceOpportunityReport.ToList();
        }

        [Fact]
        public void Then_Saved_Referral_Is_Included()
        {
            _result.Should().NotBeNull();
            _result.SingleOrDefault(o => o.OpportunityItemId == _savedReferralOpportunityItem.Id).Should().NotBeNull();
        }

        [Fact]
        public void Then_Saved_ProvisionGap_Is_Included()
        {
            _result.Should().NotBeNull();
            _result.SingleOrDefault(o => o.OpportunityItemId == _savedProvisionGapOpportunityItem.Id).Should().NotBeNull();
        }

        [Fact]
        public void Then_Unsaved_Referral_Is_NOT_Included()
        {
            _result.Should().NotBeNull();
            _result.SingleOrDefault(o => o.OpportunityItemId == _unsavedReferralOpportunityItem.Id).Should().BeNull();
        }

        [Fact]
        public void Then_Unsaved_ProvisionGap_Is_NOT_Included()
        {
            _result.Should().NotBeNull();
            _result.SingleOrDefault(o => o.OpportunityItemId == _unsavedProvisionGapOpportunityItem.Id).Should().BeNull();
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
