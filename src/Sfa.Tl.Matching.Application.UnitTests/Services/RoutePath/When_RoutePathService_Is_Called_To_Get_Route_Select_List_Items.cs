using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Select_List_Items : IClassFixture<RoutePathTestFixture>
    {
        private readonly IList<SelectListItem> _result;

        public When_RoutePathService_Is_Called_To_Get_Route_Select_List_Items(RoutePathTestFixture testFixture)
        {
            _result = testFixture.RoutePathService.GetRouteSelectListItemsAsync().GetAwaiter().GetResult();
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
            firstResult.Value.Should().Be("1");
            firstResult.Text.Should().Be("Route 1");

            var secondResult = _result.Skip(1).First();
            secondResult.Value.Should().Be("2");
            secondResult.Text.Should().Be("Route 2");
        }
    }
}
