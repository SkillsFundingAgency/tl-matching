using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath.Builders
{
    public class ValidRouteListBuilder
    {
        public IList<Route> Build() => new List<Route>
        {
            new Route
            {
                Id = 1,
                Name = "Route 1",
                Keywords = "Keyword1",
                Summary = "Route 1 summary"
            },
            new Route
            {
                Id = 2,
                Name = "Route 2",
                Keywords = "Keyword2",
                Summary = "Route 2 summary"
            }
        };
    }
}
