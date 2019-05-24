using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.backgroundProcessHistory
{
    public class When_backgroundProcessHistoryRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_backgroundProcessHistoryRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BackgroundProcessHistory>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidbackgroundProcessHistoryListBuilder().Build();

                var repository = new GenericRepository<Domain.Models.BackgroundProcessHistory>(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}