using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMappingRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.QualificationRoutePathMapping _result;

        public When_QualificationRoutePathMappingRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<QualificationRoutePathMappingRepository>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidQualificationRoutePathMappingBuilder().Build());
                dbContext.SaveChanges();

                var repository = new QualificationRoutePathMappingRepository(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_RoutePathMapping_Is_Returned() =>
            _result.Should().BeNull();
    }
}