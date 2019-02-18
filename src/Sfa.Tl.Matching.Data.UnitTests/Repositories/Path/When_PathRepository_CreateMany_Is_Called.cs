using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_PathRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<PathRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidPathListBuilder().Build();

                var repository = new PathRepository(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}