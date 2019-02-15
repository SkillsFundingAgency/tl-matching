using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Builders
{
    internal class ValidRoutePathMappingBuilder
    {
        private readonly Domain.Models.RoutePathMapping _routePathMapping;

        public ValidRoutePathMappingBuilder()
        {
            _routePathMapping = new Domain.Models.RoutePathMapping
            {
                Id = 1,
                LarsId = RoutePathMappingConstants.LarsId,
                Title = RoutePathMappingConstants.Title,
                ShortTitle = RoutePathMappingConstants.ShortTitle,
                PathId = RoutePathMappingConstants.PathId,
                Source = RoutePathMappingConstants.Source,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public Domain.Models.RoutePathMapping Build() =>
            _routePathMapping;
    }
}
