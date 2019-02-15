using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.UnitTests.Data.Provider.Builders;
using Sfa.Tl.Matching.Data.Repositories;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Provider
{
    public class When_ProviderRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_ProviderRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<ProviderRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidProviderListBuilder().Build();

                var repository = new ProviderRepository(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}