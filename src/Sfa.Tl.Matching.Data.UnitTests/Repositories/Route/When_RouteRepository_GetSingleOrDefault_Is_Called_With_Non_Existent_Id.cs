using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id : IClassFixture<RouteTestFixture>
    {
        private readonly Domain.Models.Route _result;

        public When_RouteRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id(RouteTestFixture testFixture)
        {
            _result = testFixture.Repository.GetSingleOrDefaultAsync(x => x.Id == 99)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_No_Route_Is_Returned() =>
            _result.Should().BeNull();
    }
}