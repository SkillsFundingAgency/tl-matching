using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.MatchingServiceProviderEmployerReport
{
    public class When_MatchingServiceProviderEmployerReport_Is_Called_For_Saved_And_Completed_Referral_And_ProvisionGap : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceProviderEmployerReport> _result;
        private readonly OpportunityItem _savedProvisionGapOpportunityItem;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceProviderEmployerReport_Is_Called_For_Saved_And_Completed_Referral_And_ProvisionGap()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            ClearData();

            var employer = _employerBuilder.CreateEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreateProvider();
            
            _savedReferralOpportunityItem = _opportunityBuilder.CreateReferralOpportunityItem(true, true, provider.ProviderVenue.First().Id);
            _savedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(true, true);

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem, _savedProvisionGapOpportunityItem });

            _result = _dbContext.MatchingServiceProviderEmployerReport.ToList();
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

        [Fact]
        public void Then_ProvisionGap_Is_Not_Selected()
        {
            var item = _result.SingleOrDefault(o => o.OpportunityItemId == _savedProvisionGapOpportunityItem.Id);
            item.Should().BeNull();
        }

        [Fact]
        public void Then_Referral_Is_Selected()
        {
            var item = _result.SingleOrDefault(o => o.OpportunityItemId == _savedReferralOpportunityItem.Id);
            item.Should().NotBeNull();
        }

    }
}
