using System;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class When_BankHolidayRepository_Update_Is_Called : IClassFixture<BankHolidayTestFixture>
    {
        private readonly Domain.Models.BankHoliday _result;

        public When_BankHolidayRepository_Update_Is_Called(BankHolidayTestFixture testFixture)
        {
            var entity = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();

            entity.Date = DateTime.Parse("2019-08-29");
            entity.Title = "Updated bank holiday";

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
            _result.Date.Should().Be(DateTime.Parse("2019-08-29"));
            _result.Title.Should().BeEquivalentTo("Updated bank holiday");

            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.ModifiedBy.Should().Be("UpdateTestUser");
            _result.ModifiedOn.Should().Be(DateTime.Parse("2019/11/01 12:30"));
        }
    }
}
