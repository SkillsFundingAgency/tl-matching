using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class When_BackgroundProcessHistoryRepository_GetMany_Is_Called : IClassFixture<BackgroundProcessHistoryTestFixture>
    {
        private readonly IEnumerable<Domain.Models.BackgroundProcessHistory> _result;

        public When_BackgroundProcessHistoryRepository_GetMany_Is_Called(BackgroundProcessHistoryTestFixture testFixture)
        {
            _result = testFixture.Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_BackgroundProcessHistories_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_BackgroundProcessHistory_Fields_Have_Expected_Values()
        {
            _result.First().Id.Should().Be(1);
            _result.First().ProcessType.Should().Be("Process 1");
            _result.First().Status.Should().Be(BackgroundProcessHistoryStatus.Pending.ToString());
            _result.First().StatusMessage.Should().Be("Status Message 1");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}