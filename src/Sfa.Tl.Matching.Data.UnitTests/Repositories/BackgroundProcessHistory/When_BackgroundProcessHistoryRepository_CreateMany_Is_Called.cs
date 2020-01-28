using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class When_BackgroundProcessHistoryRepository_CreateMany_Is_Called : IClassFixture<BackgroundProcessHistoryTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_BackgroundProcessHistoryRepository_CreateMany_Is_Called(BackgroundProcessHistoryTestFixture testFixture)
        {
            testFixture.Builder
                .ClearData()
                .CreateBackgroundProcessHistory(3, 1, "Process 3", "Pending", createdBy: EntityCreationConstants.CreatedByUser)
                .CreateBackgroundProcessHistory(4, 2, "Process 4", "Pending", createdBy: EntityCreationConstants.CreatedByUser);

            _result = testFixture.Repository.CreateManyAsync(testFixture.Builder.BackgroundProcessHistories)
                    .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.BackgroundProcessHistory.Count();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);

        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}