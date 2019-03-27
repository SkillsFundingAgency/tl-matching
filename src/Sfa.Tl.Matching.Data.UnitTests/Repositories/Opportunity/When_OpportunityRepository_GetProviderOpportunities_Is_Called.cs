using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetProviderOpportunities_Is_Called
    {
        private readonly IList<OpportunityReferralDto> _result;

        public When_OpportunityRepository_GetProviderOpportunities_Is_Called()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidOpportunityReferralBuilder().Build());
                dbContext.SaveChanges();

                var repository = new OpportunityRepository(logger, dbContext);
                _result = repository.GetProviderOpportunities(1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_One_Item_Is_Returned() =>
            _result.Count.Should().Be(1);

        [Fact]
        public void Then_OpportunityId_Is_Returned() =>
            _result.First().OpportunityId.Should().Be(1);

        [Fact]
        public void Then_ReferralId_Is_Returned() =>
            _result.First().ReferralId.Should().Be(1);

        [Fact]
        public void Then_ProviderName_Is_Returned() =>
            _result.First().ProviderName.Should().BeEquivalentTo("ProviderName");

        [Fact]
        public void Then_ProviderPrimaryContact_Is_Returned() =>
            _result.First().ProviderPrimaryContact.Should().BeEquivalentTo("PrimaryContact");

        [Fact]
        public void Then_ProviderPrimaryContactEmail_Is_Returned() =>
            _result.First().ProviderPrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");

        [Fact]
        public void Then_ProviderSecondaryContactEmail_Is_Returned() =>
            _result.First().ProviderSecondaryContactEmail.Should().BeEquivalentTo("secondary@contact.co.uk");

        [Fact]
        public void Then_ProviderVenuePostcode_Is_Returned() =>
            _result.First().ProviderVenuePostcode.Should().BeEquivalentTo("AA1 1AA");

        [Fact]
        public void Then_RouteName_Is_Returned() =>
            _result.First().RouteName.Should().BeEquivalentTo("Test Route");

        [Fact]
        public void Then_SearchRadius_Is_Returned()
            => _result.First().SearchRadius.Should().Be(10);

        [Fact]
        public void Then_Postcode_Is_Returned() =>
            _result.First().Postcode.Should().BeEquivalentTo("AA1 1AA");

        [Fact]
        public void Then_JobTitle_Is_Returned() =>
            _result.First().JobTitle.Should().BeEquivalentTo("Testing Job Title");

        [Fact]
        public void Then_EmployerName_Is_Returned()
            => _result.First().EmployerName.Should().BeEquivalentTo("Employer");

        [Fact]
        public void Then_EmployerContact_Is_Returned()
            => _result.First().EmployerContact.Should().BeEquivalentTo("Employer Contact");

        [Fact]
        public void Then_EmployerContactPhone_Is_Returned()
            => _result.First().EmployerContactPhone.Should().BeEquivalentTo("020 123 4567");

        [Fact]
        public void Then_EmployerContactEmail_Is_Returned()
            => _result.First().EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");

        [Fact]
        public void Then_PlacementsKnown_Is_Returned()
            => _result.First().PlacementsKnown.Should().Be(true);

        [Fact]
        public void Then_Placements_Is_Returned()
            => _result.First().Placements.Should().Be(3);

        [Fact]
        public void Then_CreatedBy_Is_Returned() =>
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
    }
}