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
    public class When_MatchingServiceProviderOpportunityReport_Is_Called_For_PipelineOpportunity_Opportunity_And_Single_Provider_Venue : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceProviderOpportunityReport> _result;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;

        public When_MatchingServiceProviderOpportunityReport_Is_Called_For_PipelineOpportunity_Opportunity_And_Single_Provider_Venue()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            ClearData();

            var employer = _employerBuilder.CreaeEmployer(Guid.NewGuid());
            var provider = _providerBuilder.CreaeProvider();

            var savedReferralOpportunityItem1 = _opportunityBuilder.CreaeReferralOpportunityItem(true, false, provider.ProviderVenue.Select(pv => pv.Id).ToArray());

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { savedReferralOpportunityItem1 });

            _result = _dbContext.MatchingServiceProviderOpportunityReport.ToList();
        }

        [Fact]
        public void Then_OpportunityItem_Is_Not_Selected()
        {
            _result.Should().BeNullOrEmpty();
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