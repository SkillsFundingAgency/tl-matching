using System;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class When_BankHolidayRepository_GetSingleOrDefault_Is_Called : IClassFixture<BankHolidayTestFixture>
    {
        private readonly Domain.Models.BankHoliday _result;

        public When_BankHolidayRepository_GetSingleOrDefault_Is_Called(BankHolidayTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Date.Should().BeSameDateAs(new DateTime(2019, 1, 1));
            _result.Title.Should().BeEquivalentTo("Bank Holiday 1");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}