using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_CreateMany_Is_Called : IClassFixture<PathTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_PathRepository_CreateMany_Is_Called(PathTestFixture testFixture)
        {
            testFixture.Builder
                .ClearData()
                .CreatePath(3, "Path 3", createdBy: EntityCreationConstants.CreatedByUser)
                .CreatePath(4, "Path 4", createdBy: EntityCreationConstants.CreatedByUser);

            _result = testFixture.Repository.CreateManyAsync(testFixture.Builder.Paths)
                .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.Path.Count();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}