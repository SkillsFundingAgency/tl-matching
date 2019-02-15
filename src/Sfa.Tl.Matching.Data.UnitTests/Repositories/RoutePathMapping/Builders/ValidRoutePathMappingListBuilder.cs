using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.UnitTests.Data.RoutePathMapping.Builders
{
    internal class ValidRoutePathMappingListBuilder
    {
        private readonly IList<Domain.Models.RoutePathMapping> _routePathMappings;

        public ValidRoutePathMappingListBuilder()
        {
            _routePathMappings =
                new List<Domain.Models.RoutePathMapping>
                {
                    new Domain.Models.RoutePathMapping
                    {
                        Id = 1,
                        LarsId = "0000001X",
                        Title = "Test title 1",
                        ShortTitle = "Test short title 1",
                        PathId = 1,
                        Source = "Test"
                    },
                    new Domain.Models.RoutePathMapping
                    {
                        Id = 2,
                        LarsId = "0000002X",
                        Title = "Test title 2",
                        ShortTitle = "Test short title 2",
                        PathId = 2,
                        Source = "Test"
                    }
                };
        }

        public IEnumerable<Domain.Models.RoutePathMapping> Build() =>
            _routePathMappings;
    }
}
