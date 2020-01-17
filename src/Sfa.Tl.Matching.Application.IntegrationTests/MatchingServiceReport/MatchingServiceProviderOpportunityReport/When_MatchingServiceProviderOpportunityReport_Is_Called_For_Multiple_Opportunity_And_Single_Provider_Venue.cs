using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.MatchingServiceProviderOpportunityReport
{
    public class When_MatchingServiceProviderOpportunityReport_Is_Called_For_Multiple_Opportunity_And_Single_Provider_Venue : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly List<Domain.Models.MatchingServiceProviderOpportunityReport> _result;
        private readonly OpportunityBuilder _opportunityBuilder;
        private readonly ProviderBuilder _providerBuilder;
        private readonly EmployerBuilder _employerBuilder;
        private readonly Provider _provider;

        public When_MatchingServiceProviderOpportunityReport_Is_Called_For_Multiple_Opportunity_And_Single_Provider_Venue()
        {
            _dbContext = new TestConfiguration().GetDbContext();

            _opportunityBuilder = new OpportunityBuilder(_dbContext);
            _providerBuilder = new ProviderBuilder(_dbContext);
            _employerBuilder = new EmployerBuilder(_dbContext);

            ClearData();

            var employer = _employerBuilder.CreateEmployer(Guid.NewGuid());
            _provider = _providerBuilder.CreateProvider();

            var savedReferralOpportunityItem1 = _opportunityBuilder.CreateReferralOpportunityItem(true, true, _provider.ProviderVenue.Select(pv => pv.Id).ToArray());
            var savedReferralOpportunityItem2 = _opportunityBuilder.CreateReferralOpportunityItem(true, true, _provider.ProviderVenue.Select(pv => pv.Id).ToArray());

            _opportunityBuilder.CreateOpportunity(employer.CrmId, new List<OpportunityItem> { savedReferralOpportunityItem1, savedReferralOpportunityItem2 });

            _result = _dbContext.MatchingServiceProviderOpportunityReport.ToList();
        }

        [Fact]
        public void Then_ProviderVenue_Should_Have_One_OpportunityItem()
        {
            _result.Should().NotBeNull();
            var item1 = _result.SingleOrDefault(o => o.ProviderVenuePostCode == _provider.ProviderVenue.ElementAt(0).Postcode);
            item1.Should().NotBeNull();
            item1?.OpportunityItemCount.Should().Be(2);
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