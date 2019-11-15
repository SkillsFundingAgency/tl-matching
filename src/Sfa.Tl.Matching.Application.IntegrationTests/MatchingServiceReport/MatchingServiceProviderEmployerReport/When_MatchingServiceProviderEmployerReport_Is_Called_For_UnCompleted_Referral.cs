using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.MatchingServiceProviderEmployerReport
{
    public class When_MatchingServiceProviderEmployerReport_Is_Called_For_UnCompleted_Referral : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceProviderEmployerReport> _result;
        private readonly OpportunityItem _savedReferralOpportunityItem;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceProviderEmployerReport_Is_Called_For_UnCompleted_Referral()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            ClearData();

            var employer = _employerBuilder.CreateEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreateProvider();
            
            _savedReferralOpportunityItem = _opportunityBuilder.CreateReferralOpportunityItem(true, false, provider.ProviderVenue.First().Id);

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { _savedReferralOpportunityItem });

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
        public void Then_Referral_Is_Not_Selected()
        {
            var item = _result.SingleOrDefault(o => o.OpportunityItemId == _savedReferralOpportunityItem.Id);
            item.Should().BeNull();
        }
    }
}