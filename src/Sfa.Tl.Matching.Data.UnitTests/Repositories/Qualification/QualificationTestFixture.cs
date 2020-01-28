using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification
{
    public class QualificationTestFixture : IDisposable
    {
        public QualificationBuilder Builder { get; }
        public MatchingDbContext MatchingDbContext;

        public IRepository<Domain.Models.Qualification> Repository { get; }

        public QualificationTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Qualification>>>();

            MatchingDbContext = InMemoryDbContext.Create();

            Builder = new QualificationBuilder(MatchingDbContext)
                .CreateQualifications(2, createdBy: EntityCreationConstants.CreatedByUser,
                    createdOn: EntityCreationConstants.CreatedOn,
                    modifiedBy: EntityCreationConstants.ModifiedByUser,
                    modifiedOn: EntityCreationConstants.ModifiedOn)
                .SaveData();

            Repository = new GenericRepository<Domain.Models.Qualification>(logger, MatchingDbContext);
        }

        public void Dispose()
        {
            Builder?.ClearData();
            MatchingDbContext?.Dispose();
        }
    }
}
