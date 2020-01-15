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
    public class When_MatchingServiceOpportunityReport_Is_Called_For_Pipeline_Opportunity : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceOpportunityReport> _result;
        private readonly OpportunityItem _savedProvisionGapOpportunityItem;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceOpportunityReport_Is_Called_For_Pipeline_Opportunity()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            ClearData();

            var employer = _employerBuilder.CreateEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreateProvider();
            
            _savedReferralOpportunityItem = _opportunityBuilder.CreateReferralOpportunityItem(true, false, provider.ProviderVenue.First().Id);
            _savedProvisionGapOpportunityItem = _opportunityBuilder.CreateProvisionGapOpportunityItem(true, false);

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem, _savedProvisionGapOpportunityItem });

            _result = _dbContext.MatchingServiceOpportunityReport.ToList();
        }

        [Fact]
        public void Then_Pipeline_Referral_Is_Included()
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
            item?.NoSuitableStudent.Should().BeTrue();
            item?.HadBadExperience.Should().BeTrue();
            item?.ProvidersTooFarAway.Should().BeTrue();
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