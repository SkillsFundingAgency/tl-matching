using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetProviderOpportunities_Is_Called
    {
        private readonly int _opportunityId;
        private readonly int _referralId;
        private readonly int _opportunityItemId;
        private readonly IList<OpportunityReferralDto> _result;

        public When_OpportunityRepository_GetProviderOpportunities_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            var dbContext = InMemoryDbContext.Create();

            var opportunity = new ValidOpportunityBuilder()
                .AddEmployer()
                .AddReferrals(true)
                .Build();
            
            var opportunityItem = opportunity.OpportunityItem.First();
            opportunityItem.IsSelectedForReferral = true;

            dbContext.Add(opportunity);
            dbContext.SaveChanges();

            _opportunityId = opportunity.Id;
            _referralId = opportunityItem.Referral.First().Id;
            _opportunityItemId = opportunity.OpportunityItem.First().Id;
            
            var repository = new OpportunityRepository(logger, dbContext);
            _result = repository.GetProviderOpportunitiesAsync(
                    1, 
                    new List<int> { _opportunityItemId })
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_One_Item_Is_Returned() =>
            _result.Count.Should().Be(1);

        [Fact]
        public void Then_OpportunityReferralDto_Fields_Are_As_Expected()
        {
            var dto = _result.First();
            dto.OpportunityId.Should().Be(_opportunityId);
            dto.OpportunityItemId.Should().Be(_opportunityItemId);
            dto.ReferralId.Should().Be(_referralId);
            dto.ProviderName.Should().BeEquivalentTo("ProviderName");
            dto.ProviderPrimaryContact.Should().BeEquivalentTo("PrimaryContact");
            dto.ProviderPrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");
            dto.ProviderSecondaryContactEmail.Should().BeEquivalentTo("secondary@contact.co.uk");
            dto.ProviderVenuePostcode.Should().BeEquivalentTo("AA1 1AA");
            dto.RouteName.Should().BeEquivalentTo("Test Route");
            dto.Postcode.Should().BeEquivalentTo("CV1 2WT");
            dto.JobRole.Should().BeEquivalentTo("Automation Tester");
            dto.CompanyName.Should().BeEquivalentTo("Company");
            dto.EmployerContact.Should().BeEquivalentTo("Employer Contact");
            dto.EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");
            dto.EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");
            dto.PlacementsKnown.Should().BeTrue();
            dto.Placements.Should().Be(5);
            dto.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
        }
    }
}