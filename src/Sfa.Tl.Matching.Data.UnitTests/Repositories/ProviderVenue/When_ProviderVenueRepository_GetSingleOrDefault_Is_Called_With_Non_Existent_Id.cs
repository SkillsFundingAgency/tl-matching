using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue
{
    public class When_ProviderVenueRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.ProviderVenue _result;

        public When_ProviderVenueRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<ProviderVenueRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidProviderVenueBuilder().Build());
                dbContext.SaveChanges();

                var repository = new ProviderVenueRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_ProviderVenue_Is_Returned() =>
            _result.Should().BeNull();
    }
}