using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.BackgroundProcessHistory
{
    public class BackgroundProcessHistoryTestFixture : IDisposable
    {
        public BackgroundProcessHistoryBuilder Builder { get; }
        public MatchingDbContext MatchingDbContext;

        public IRepository<Domain.Models.BackgroundProcessHistory> Repository { get; }

        public BackgroundProcessHistoryTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.BackgroundProcessHistory>>>();

            MatchingDbContext = InMemoryDbContext.Create();

            Builder = new BackgroundProcessHistoryBuilder(MatchingDbContext)
                .CreateBackgroundProcessHistories(2, createdBy: EntityCreationConstants.CreatedByUser,
                    createdOn: EntityCreationConstants.CreatedOn,
                    modifiedBy: EntityCreationConstants.ModifiedByUser,
                    modifiedOn: EntityCreationConstants.ModifiedOn)
                .SaveData();

            Repository = new GenericRepository<Domain.Models.BackgroundProcessHistory>(logger, MatchingDbContext);
        }

        public void Dispose()
        {
            Builder?.ClearData();
            MatchingDbContext?.Dispose();
        }
    }
}
