using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class When_BankHolidayRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id : IClassFixture<BankHolidayTestFixture>
    {
        private readonly Domain.Models.BankHoliday _result;

        public When_BankHolidayRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id(BankHolidayTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 99)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_BankHoliday_Is_Returned() =>
            _result.Should().BeNull();
    }
}