using System;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class When_BackgroundProcessHistoryRepository_Update_Is_Called : IClassFixture<BackgroundProcessHistoryTestFixture>
    {
        private readonly Domain.Models.BackgroundProcessHistory _result;

        public When_BackgroundProcessHistoryRepository_Update_Is_Called(BackgroundProcessHistoryTestFixture testFixture)
        {
            var entity = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();

            entity.RecordCount = 10;
            entity.Status = BackgroundProcessHistoryStatus.Complete.ToString();
            entity.StatusMessage = "UpdatedStatus Message";

            entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
            entity.ModifiedBy = "UpdateTestUser";

            testFixture.Repository.UpdateAsync(entity).GetAwaiter().GetResult();

            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.RecordCount.Should().Be(10);
            _result.Status.Should().Be(BackgroundProcessHistoryStatus.Complete.ToString());
            _result.StatusMessage.Should().Be("UpdatedStatus Message");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
