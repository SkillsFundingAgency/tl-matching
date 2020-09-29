using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_QualificationRouteMappingRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<QualificationRouteMappingRepository>>();

            using var dbContext = InMemoryDbContext.Create();
            var data = new ValidQualificationRouteMappingListBuilder().Build();

            var repository = new QualificationRouteMappingRepository(logger, dbContext);
            _result = repository.CreateManyAsync(data)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            //This is returning four because qualification objects are also getting inserted
            _result.Should().Be(4);
    }
}