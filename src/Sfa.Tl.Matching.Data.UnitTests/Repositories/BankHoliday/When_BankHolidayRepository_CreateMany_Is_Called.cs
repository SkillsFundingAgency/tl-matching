using System;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class When_BankHolidayRepository_CreateMany_Is_Called : IClassFixture<BankHolidayTestFixture>
    {
        private readonly int _result;
        private readonly int _rowsInDb;

        public When_BankHolidayRepository_CreateMany_Is_Called(BankHolidayTestFixture testFixture)
        {
            testFixture.Builder
                .ClearData()
                .CreateBankHoliday(3, DateTime.Parse("2019-08-26"), "Summer bank holiday", createdBy: EntityCreationConstants.CreatedByUser)
                .CreateBankHoliday(4, DateTime.Parse("2020-01-01"), "New Year's Day", createdBy: EntityCreationConstants.CreatedByUser);

            _result = testFixture.Repository.CreateManyAsync(testFixture.Builder.BankHolidays)
                    .GetAwaiter().GetResult();

            _rowsInDb = testFixture.MatchingDbContext.BankHoliday.Count();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
        
        [Fact]
        public void Then_Two_Records_Should_Be_In_The_Database() =>
            _rowsInDb.Should().Be(2);
    }
}