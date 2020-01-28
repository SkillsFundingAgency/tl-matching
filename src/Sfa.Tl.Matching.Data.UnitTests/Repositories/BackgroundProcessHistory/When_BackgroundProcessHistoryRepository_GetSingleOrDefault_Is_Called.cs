using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class When_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called : IClassFixture<BackgroundProcessHistoryTestFixture>
    {
        private readonly Domain.Models.BackgroundProcessHistory _result;

        public When_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called(BackgroundProcessHistoryTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.RecordCount.Should().Be(1);
            _result.ProcessType.Should().Be("Process 1");
            _result.Status.Should().Be(BackgroundProcessHistoryStatus.Pending.ToString());
            _result.StatusMessage.Should().Be("Status Message 1");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}