using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue
{
    public class When_ProviderVenueRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.ProviderVenue _result;

        public When_ProviderVenueRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<ProviderVenueRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidProviderVenueListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new ProviderVenueRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_ProviderVenue_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_ProviderVenue_ProviderId_Is_Returned() =>
            _result.ProviderId.Should().Be(10000546);

        [Fact]
        public void Then_ProviderVenue_Postcode_Is_Returned() =>
            _result.Postcode.Should().BeEquivalentTo("AA1 1AA");

        [Fact]
        public void Then_ProviderVenue_Town_Is_Returned()
            => _result.Town.Should().BeEquivalentTo("Town");

        [Fact]
        public void Then_ProviderVenue_County_Is_Returned()
            => _result.County.Should().BeEquivalentTo("County");

        [Fact]
        public void Then_ProviderVenue_Status_Is_Returned()
            => _result.Status.Should().Be(true);

        [Fact]
        public void Then_ProviderVenue_StatusReason_Is_Returned()
            => _result.StatusReason.Should().BeEquivalentTo("Reason");

        [Fact]
        public void Then_ProviderVenue_Latitude_Is_Returned()
            => _result.Latitude.Should().Be(52.648869M);

        [Fact]
        public void Then_ProviderVenue_Longitude_Is_Returned()
            => _result.Longitude.Should().Be(-2.095574M);

        [Fact]
        public void Then_ProviderVenue_Source_Is_Returned() =>
            _result.Source.Should().BeEquivalentTo("Test");
        
        [Fact]
        public void Then_ProviderVenue_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_ProviderVenue_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_ProviderVenue_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().Be(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_ProviderVenue_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}