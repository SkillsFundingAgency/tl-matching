using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class RouteTestFixture : IDisposable
    {
        public RouteBuilder Builder { get; }
        public MatchingDbContext MatchingDbContext;

        public IRepository<Domain.Models.Route> Repository { get; }

        public RouteTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Route>>>();

            MatchingDbContext = InMemoryDbContext.Create();

            Builder = new RouteBuilder(MatchingDbContext)
                .CreateRoutes(2, createdBy: EntityCreationConstants.CreatedByUser,
                    createdOn: EntityCreationConstants.CreatedOn,
                    modifiedBy: EntityCreationConstants.ModifiedByUser,
                    modifiedOn: EntityCreationConstants.ModifiedOn)
                .SaveData();

            Repository = new GenericRepository<Domain.Models.Route>(logger, MatchingDbContext);
        }

        public void Dispose()
        {
            Builder?.ClearData();
            MatchingDbContext?.Dispose();
        }
    }
}
