using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMappingRepository_CreateMany_Is_Called
    {
        private readonly int _result;

        public When_QualificationRoutePathMappingRepository_CreateMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<QualificationRoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new ValidQualificationRoutePathMappingListBuilder().Build();

                var repository = new QualificationRoutePathMappingRepository(logger, dbContext);
                _result = repository.CreateMany(data)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            //This is returning four because qualification objects are also getting inserted
            _result.Should().Be(4);
    }
}