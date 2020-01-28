using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BankHoliday
{
    public class BankHolidayTestFixture : IDisposable
    {
        public BankHolidayBuilder Builder { get; }
        public MatchingDbContext MatchingDbContext;

        public IRepository<Domain.Models.BankHoliday> Repository { get; }

        public BankHolidayTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BankHoliday>>>();

            MatchingDbContext = InMemoryDbContext.Create();

            Builder = new BankHolidayBuilder(MatchingDbContext)
                .CreateBankHolidays(2, createdBy: EntityCreationConstants.CreatedByUser,
                    createdOn: EntityCreationConstants.CreatedOn,
                    modifiedBy: EntityCreationConstants.ModifiedByUser,
                    modifiedOn: EntityCreationConstants.ModifiedOn)
                .SaveData();

            Repository = new GenericRepository<Domain.Models.BankHoliday>(logger, MatchingDbContext);
        }

        public void Dispose()
        {
            Builder?.ClearData();
            MatchingDbContext?.Dispose();
        }
    }
}
