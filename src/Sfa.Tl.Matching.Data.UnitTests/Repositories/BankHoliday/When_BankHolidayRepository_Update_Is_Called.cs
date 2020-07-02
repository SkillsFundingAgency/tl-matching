using System;
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
    public class When_BankHolidayRepository_Update_Is_Called
    {
        private readonly Domain.Models.BankHoliday _result;

        public When_BankHolidayRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BankHoliday>>>();

            using var dbContext = InMemoryDbContext.Create();
            var entity = new ValidBankHolidayListBuilder().Build().First();
            dbContext.Add(entity);
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.BankHoliday>(logger, dbContext);

            entity.Date = DateTime.Parse("2019-08-29");
            entity.Title = "Updated bank holiday";

            entity.ModifiedOn = new DateTime(2019, 11, 01, 12, 30, 00);
            entity.ModifiedBy = "UpdateTestUser";

            repository.UpdateAsync(entity).GetAwaiter().GetResult();

            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
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
