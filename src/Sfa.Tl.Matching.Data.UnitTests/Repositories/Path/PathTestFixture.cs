using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class PathTestFixture : IDisposable
    {
        public PathBuilder Builder { get; }
        public MatchingDbContext MatchingDbContext;

        public IRepository<Domain.Models.Path> Repository { get; }

        public PathTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Path>>>();

            MatchingDbContext = InMemoryDbContext.Create();

            Builder = new PathBuilder(MatchingDbContext)
                .CreatePaths(2, createdBy: EntityCreationConstants.CreatedByUser,
                    createdOn: EntityCreationConstants.CreatedOn,
                    modifiedBy: EntityCreationConstants.ModifiedByUser,
                    modifiedOn: EntityCreationConstants.ModifiedOn)
                .SaveData();

            Repository = new GenericRepository<Domain.Models.Path>(logger, MatchingDbContext);
        }

        public void Dispose()
        {
            Builder?.ClearData();
            MatchingDbContext?.Dispose();
        }
    }
}
