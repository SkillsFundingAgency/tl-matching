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
    public class When_MatchingServiceEmployerReport_Is_Called_For_Pipeline_Opportunity : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<MatchingServiceOpportunityReport> _result;
        private readonly OpportunityItem _savedProvisionGapOpportunityItem;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceEmployerReport_Is_Called_For_Pipeline_Opportunity()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            var employer = _employerBuilder.CreaeEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreaeProvider();
            
            _savedReferralOpportunityItem = _opportunityBuilder.CreaeReferralOpportunityItem(true, false, provider.ProviderVenue.First().Id);
            _savedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(true, false);

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem, _savedProvisionGapOpportunityItem });

            _result = _dbContext.MatchingServiceOpportunityReport.ToList();
        }

        [Fact]
        public void Then_Pipeline_referal_Is_included()
        {
            _result.Should().NotBeNull();
            var item = _result.SingleOrDefault(o => o.OpportunityItemId == _savedReferralOpportunityItem.Id);
            item.Should().NotBeNull();
            item?.PipelineOpportunity.Should().BeTrue();
        }

        [Fact]
        public void Then_Pipeline_ProvisionGap_Is_Included()
        {
            _result.Should().NotBeNull();
            var item = _result.SingleOrDefault(o => o.OpportunityItemId == _savedProvisionGapOpportunityItem.Id);
            item.Should().NotBeNull();
            item?.PipelineOpportunity.Should().BeTrue();
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