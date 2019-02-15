
namespace Sfa.Tl.Matching.Application.UnitTests.Data.RoutePathMapping.Builders
{
    internal class ValidRoutePathMappingBuilder
    {
        private readonly Domain.Models.RoutePathMapping _routePathMapping;

        public ValidRoutePathMappingBuilder()
        {
            _routePathMapping = new Domain.Models.RoutePathMapping
            {
                Id = 1,
                LarsId = "1234567X",
                Title = "Test title",
                ShortTitle = "Test short title",
                PathId = 2,
                Source = "Test"
            };
        }

        public Domain.Models.RoutePathMapping Build() =>
            _routePathMapping;
    }
}
