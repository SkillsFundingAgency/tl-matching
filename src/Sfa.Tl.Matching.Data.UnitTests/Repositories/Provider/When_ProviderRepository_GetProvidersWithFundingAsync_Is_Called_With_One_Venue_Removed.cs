using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider
{
    public class When_ProviderRepository_GetProvidersWithFundingAsync_Is_Called_With_One_Venue_Removed
    {
        private readonly IList<ProviderWithFundingDto> _result;

        public When_ProviderRepository_GetProvidersWithFundingAsync_Is_Called_With_One_Venue_Removed()
        {
            var logger = Substitute.For<ILogger<ProviderRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidProviderWithFundingBuilder().BuildWithRemovedProviderVenue());
                dbContext.SaveChanges();

                var repository = new ProviderRepository(logger, dbContext);
                _result = repository.GetProvidersWithFundingAsync()
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_One_Item_Is_Returned() =>
            _result.Count.Should().Be(1);

        [Fact]
        public void Then_Provider_Fields_Are_As_Expected()
        {
            var provider = _result.First();
            provider.Id.Should().Be(1);
            provider.Name.Should().Be("ProviderName");
            provider.PrimaryContact.Should().BeEquivalentTo("PrimaryContact");
            provider.PrimaryContactEmail.Should().BeEquivalentTo("primary@contact.co.uk");
            provider.PrimaryContactPhone.Should().BeEquivalentTo("01777757777");
            provider.SecondaryContact.Should().BeEquivalentTo("SecondaryContact");
            provider.SecondaryContactEmail.Should().BeEquivalentTo("secondary@contact.co.uk");
            provider.SecondaryContactPhone.Should().BeEquivalentTo("01888801234");
            provider.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
        }

        [Fact]
        public void Then_One_ProviderVenue_Is_Returned() =>
            _result.First().ProviderVenues.Count().Should().Be(1);

        [Fact]
        public void Then_ProviderVenue_Fields_Are_As_Expected()
        {
            var providerVenue = _result.First().ProviderVenues.First();
            providerVenue.Postcode.Should().BeEquivalentTo("AA2 2AA");
        }
    }
}