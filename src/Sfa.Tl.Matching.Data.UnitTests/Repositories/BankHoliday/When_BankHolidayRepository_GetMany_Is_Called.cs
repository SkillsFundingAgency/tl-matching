using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class When_BankHolidayRepository_GetMany_Is_Called : IClassFixture<BankHolidayTestFixture>
    {
        private readonly IEnumerable<Domain.Models.BankHoliday> _result;

        public When_BankHolidayRepository_GetMany_Is_Called(BankHolidayTestFixture testFixture)
        {
            _result = testFixture.Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_BankHolidays_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_BankHoliday_Fields_Have_Expected_Values()
        {
            _result.First().Id.Should().Be(1);
            _result.First().Date.Should().BeSameDateAs(new DateTime(2019, 1, 1));
            _result.First().Title.Should().BeEquivalentTo("Bank Holiday 1");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}