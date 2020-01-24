using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.QualificationRouteMapping
{
    public class When_QualificationRouteMappingRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id : IClassFixture<QualificationRouteMappingTestFixture>
    {
        private readonly Domain.Models.QualificationRouteMapping _result;

        public When_QualificationRouteMappingRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id(QualificationRouteMappingTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 99)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_RoutePathMapping_Is_Returned() =>
            _result.Should().BeNull();
    }
}