using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Summary : IClassFixture<RoutePathTestFixture>
    {
        private readonly IList<RouteSummaryViewModel> _result;

        public When_RoutePathService_Is_Called_To_Get_Route_Summary(RoutePathTestFixture testFixture)
        {
            _result = testFixture.RoutePathService.GetRouteSummaryAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(2, _result.Count);
        }

        [Fact]
        public void Then_Route_Summary_Data_Is_As_Expected()
        {
            var firstResult = _result.First();
            firstResult.Id.Should().Be(1);
            firstResult.IsSelected.Should().BeFalse();
            firstResult.Name.Should().Be("Route 1");
            firstResult.Summary.Should().Be("Route 1 summary");

            var secondResult = _result.Skip(1).First();
            secondResult.Id.Should().Be(2);
            secondResult.IsSelected.Should().BeFalse();
            secondResult.Name.Should().Be("Route 2");
            secondResult.Summary.Should().Be("Route 2 summary");
        }
    }
}
