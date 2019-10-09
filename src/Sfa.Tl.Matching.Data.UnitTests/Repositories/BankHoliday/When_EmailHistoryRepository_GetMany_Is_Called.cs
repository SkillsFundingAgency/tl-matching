using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class When_BankHolidayRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.BankHoliday> _result;

        public When_BankHolidayRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BankHoliday>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidBankHolidayListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.BankHoliday>(logger, dbContext);
                _result = repository.GetManyAsync().ToList();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_EmailHistories_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_BankHoliday_Fields_Have_Expected_Values()
        {
            _result.First().Id.Should().Be(1);
            _result.First().Date.Should().BeSameDateAs(new DateTime(2019, 8, 26));
            _result.First().Title.Should().BeEquivalentTo("Summer Bank Holiday");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}