using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class QualificationRouteMappingTestFixture : IDisposable
    {
        public QualificationRouteMappingBuilder Builder { get; }
        public MatchingDbContext MatchingDbContext;

        public IRepository<Domain.Models.QualificationRouteMapping> Repository { get; }

        public QualificationRouteMappingTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.QualificationRouteMapping>>>();

            MatchingDbContext = InMemoryDbContext.Create();

            Builder = new QualificationRouteMappingBuilder(MatchingDbContext)
                .CreateQualificationRouteMappings(2, createdBy: EntityCreationConstants.CreatedByUser,
                    createdOn: EntityCreationConstants.CreatedOn,
                    modifiedBy: EntityCreationConstants.ModifiedByUser,
                    modifiedOn: EntityCreationConstants.ModifiedOn)
                .SaveData();

            Repository = new GenericRepository<Domain.Models.QualificationRouteMapping>(logger, MatchingDbContext);
        }

        public void Dispose()
        {
            Builder?.ClearData();
            MatchingDbContext?.Dispose();
        }
    }
}
