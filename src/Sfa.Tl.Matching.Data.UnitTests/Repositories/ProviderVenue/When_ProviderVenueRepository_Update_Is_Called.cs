using System;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue
{
    public class When_ProviderVenueRepository_Update_Is_Called
    {
        private readonly Domain.Models.ProviderVenue _result;

        public When_ProviderVenueRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.ProviderVenue>>>();

            using var dbContext = InMemoryDbContext.Create();
            var entity = new ValidProviderVenueBuilder().Build();
            dbContext.Add(entity);
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.ProviderVenue>(logger, dbContext);

            entity.Name = "Updated Venue Name";
            entity.Town = "Updated Town";
            entity.County = "Updated County";
            entity.Source = "Updated";

            entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
            entity.ModifiedBy = "UpdateTestUser";

            repository.UpdateAsync(entity).GetAwaiter().GetResult();

            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.ProviderId.Should().Be(10000546);
            _result.Postcode.Should().BeEquivalentTo("AA1 1AA");
            _result.Latitude.Should().Be(52.648869M);
            _result.Longitude.Should().Be(-2.095574M);

            _result.Name.Should().BeEquivalentTo("Updated Venue Name");
            _result.Town.Should().BeEquivalentTo("Updated Town");
            _result.County.Should().BeEquivalentTo("Updated County");
            _result.Source.Should().BeEquivalentTo("Updated");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
