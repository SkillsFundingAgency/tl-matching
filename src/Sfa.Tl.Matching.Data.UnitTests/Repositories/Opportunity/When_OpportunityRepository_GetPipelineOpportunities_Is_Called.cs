using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetPipelineOpportunities_Is_Called
    {
        private readonly OpportunityReportDto _result;

        public When_OpportunityRepository_GetPipelineOpportunities_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(
                    new ValidOpportunityBuilder()
                        .AddEmployer()
                        .AddReferrals()
                        .AddProvisionGaps()
                        .Build());
                dbContext.SaveChanges();

                var repository = new OpportunityRepository(logger, dbContext);
                _result = repository.GetPipelineOpportunitiesAsync(1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_A_Valid_Item_Is_Returned() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_OpportunityReferralDto_Fields_Are_As_Expected()
        {
            _result.CompanyName.Should().BeEquivalentTo("Company");

            _result.ReferralItems.Should().NotBeNull();
            _result.ReferralItems.Count.Should().Be(1);

            _result.ReferralItems.First().Workplace.Should().Be("Coventry CV1 2WT");
            _result.ReferralItems.First().JobRole.Should().BeEquivalentTo("Automation Tester");
            _result.ReferralItems.First().PlacementsKnown.Should().BeTrue();
            _result.ReferralItems.First().Placements.Should().Be(5);
            _result.ReferralItems.First().ProviderName.Should().BeEquivalentTo("Venue name (part of Provider display name)");
            _result.ReferralItems.First().ProviderVenueTownAndPostcode.Should().Be("Town AA1 1AA");
            _result.ReferralItems.First().DistanceFromEmployer.Should().Be(3.5M);

            _result.ProvisionGapItems.Should().NotBeNull();
            _result.ProvisionGapItems.Count.Should().Be(1);

            _result.ProvisionGapItems.First().Workplace.Should().Be("London SW1 1AA");
            _result.ProvisionGapItems.First().JobRole.Should().BeEquivalentTo("Unknown job role");
            _result.ProvisionGapItems.First().PlacementsKnown.Should().BeTrue();
            _result.ProvisionGapItems.First().Placements.Should().Be(2);
            _result.ProvisionGapItems.First().Reason.Should().Be(
                "Employer had a bad experience with them, " +
                "Providers do not have students doing the right course, " +
                "Providers were too far away");
        }
    }
}