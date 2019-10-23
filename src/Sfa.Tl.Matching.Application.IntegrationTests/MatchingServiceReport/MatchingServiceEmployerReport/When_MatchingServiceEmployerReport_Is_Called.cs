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
    public class When_MatchingServiceEmployerReport_Is_Called_For_Saved_OpportunityOpportunities : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<MatchingServiceOpportunityReport> _result;
        private readonly OpportunityItem _savedProvisionGapOpportunityItem;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityItem _unsavedProvisionGapOpportunityItem;
        private readonly OpportunityItem _unsavedReferralOpportunityItem;

        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceEmployerReport_Is_Called_For_Saved_OpportunityOpportunities()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            var employer = _employerBuilder.CreaeEmployer(Guid.NewGuid());
            var provider1 = _providerBuilder.CreaeProvider();
            var provider2 = _providerBuilder.CreaeProvider();

            _savedReferralOpportunityItem = _opportunityBuilder.CreaeReferralOpportunityItem(true, true, provider1.ProviderVenue.First().Id);
            _savedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(true, true);

            _unsavedReferralOpportunityItem = _opportunityBuilder.CreaeReferralOpportunityItem(false, false, provider2.ProviderVenue.First().Id);
            _unsavedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(false, false);

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem, _savedProvisionGapOpportunityItem, _unsavedReferralOpportunityItem, _unsavedProvisionGapOpportunityItem });

            _result = _dbContext.MatchingServiceOpportunityReport.ToList();
        }

        [Fact]
        public void Then_Save_referal_Is_included()
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
        public void Then_Unsave_referal_Is_NOT_included()
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
            _opportunityBuilder.ClearData();
            _providerBuilder.ClearData();
            _employerBuilder.ClearData();

            _dbContext?.Dispose();
        }
    }
}
