using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders
{
    internal class ValidRoutePathMappingBuilder
    {
        private readonly Domain.Models.RoutePathMapping _routePathMapping;

        public ValidRoutePathMappingBuilder()
        {
            _routePathMapping = new Domain.Models.RoutePathMapping
            {
                LarsId = RoutePathMappingConstants.LarsId,
                Title = RoutePathMappingConstants.Title,
                ShortTitle = RoutePathMappingConstants.ShortTitle,
                PathId = RoutePathMappingConstants.PathId
            };
        }

        public Domain.Models.RoutePathMapping Build() =>
            _routePathMapping;
    }
}