using System;
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
    public class When_BankHolidayRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.BankHoliday _result;

        public When_BankHolidayRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BankHoliday>>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidBankHolidayListBuilder().Build());
            dbContext.SaveChanges();

            var repository = new GenericRepository<Domain.Models.BankHoliday>(logger, dbContext);
            _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.Date.Should().BeSameDateAs(new DateTime(2019, 8, 26));
            _result.Title.Should().BeEquivalentTo("Summer Bank Holiday");
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}