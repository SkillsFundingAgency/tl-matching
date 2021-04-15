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
    public class When_OpportunityRepository_GetEmployerReferrals_Is_Called_With_On_Cdf_Provider
    {
        private readonly EmployerReferralDto _result;

        public When_OpportunityRepository_GetEmployerReferrals_Is_Called_With_On_Cdf_Provider()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.Add(
                new ValidOpportunityBuilder()
                    .AddEmployer()
                    .AddReferralsWithOneNonCdfProvider(isSelectedForReferral: true)
                    .Build());
            dbContext.SaveChanges();
                
            var repository = new OpportunityRepository(logger, dbContext);
            _result = repository.GetEmployerReferralsAsync(
                    1, 
                    new[] { 1, 2 })
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmployerReferralDto_Fields_Are_As_Expected()
        {
            _result.OpportunityId.Should().Be(1);
            _result.CompanyName.Should().BeEquivalentTo("Company");
            _result.PrimaryContact.Should().BeEquivalentTo("Employer Contact");
            _result.Phone.Should().BeEquivalentTo("020 123 4567");
            _result.Email.Should().BeEquivalentTo("employer.contact@employer.co.uk");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

            _result.WorkplaceDetails.Should().NotBeNull();
            _result.WorkplaceDetails.Count().Should().Be(1);

            _result.WorkplaceDetails.Should().NotBeNull();
            _result.WorkplaceDetails.Count().Should().Be(1);

            var workplaceDetail = _result.WorkplaceDetails.First();
            workplaceDetail.WorkplaceTown.Should().Be("Coventry");
            workplaceDetail.WorkplacePostcode.Should().Be("CV1 2WT");
            workplaceDetail.JobRole.Should().Be("Tea Taster");
            workplaceDetail.PlacementsKnown.Should().Be(true);
            workplaceDetail.Placements.Should().Be(2);
            workplaceDetail.ProviderAndVenueDetails.Should().NotBeNull();
            workplaceDetail.ProviderAndVenueDetails.Count().Should().Be(1);

            var providerReferral = workplaceDetail.ProviderAndVenueDetails.First();
            providerReferral.Town.Should().Be("Town");
            providerReferral.Postcode.Should().Be("BB2 2BB");
            providerReferral.ProviderName.Should().Be("Provider Name 2");
            providerReferral.DisplayName.Should().Be("Provider display name 2");
            providerReferral.PrimaryContact.Should().Be("PrimaryContact");
            providerReferral.PrimaryContactEmail.Should().Be("primary@contact.co.uk");
            providerReferral.PrimaryContactPhone.Should().Be("01777757777");
            providerReferral.SecondaryContact.Should().Be("SecondaryContact");
            providerReferral.SecondaryContactEmail.Should().Be("secondary@contact.co.uk");
            providerReferral.SecondaryContactPhone.Should().Be("01777757777");
            providerReferral.ProviderVenueName.Should().Be("Venue name");
        }
    }
}