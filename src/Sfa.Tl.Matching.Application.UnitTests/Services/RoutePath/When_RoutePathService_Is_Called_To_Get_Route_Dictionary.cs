using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Dictionary : IClassFixture<RoutePathTestFixture>
    {
        private readonly IDictionary<int, string> _result;

        public When_RoutePathService_Is_Called_To_Get_Route_Dictionary(RoutePathTestFixture testFixture)
        {
            _result = testFixture.RoutePathService.GetRouteDictionaryAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(2, _result.Count);
        }

        [Fact]
        public void Then_Route_Dictionary_Data_Is_As_Expected()
        {
            var firstResult = _result.First();
            firstResult.Key.Should().Be(1);
            firstResult.Value.Should().Be("Route 1");

            var secondResult = _result.Skip(1).First();
            secondResult.Key.Should().Be(2);
            secondResult.Value.Should().Be("Route 2");
        }
    }
}
