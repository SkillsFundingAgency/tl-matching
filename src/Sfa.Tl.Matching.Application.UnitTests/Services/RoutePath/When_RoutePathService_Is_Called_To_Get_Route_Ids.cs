using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Ids : IClassFixture<RoutePathTestFixture>
    {
        private readonly IList<int> _result;

        public When_RoutePathService_Is_Called_To_Get_Route_Ids(RoutePathTestFixture testFixture)
        {
            _result = testFixture.RoutePathService.GetRouteIdsAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(2, _result.Count);
        }

        [Fact]
        public void Then_The_Expected_Ids_Are_Returned()
        {
            _result[0].Should().Be(1);
            _result[1].Should().Be(2);
        }
    }
}
