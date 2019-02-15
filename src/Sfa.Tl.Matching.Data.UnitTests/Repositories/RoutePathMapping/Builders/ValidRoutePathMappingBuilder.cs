using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

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
                LarsId = "1234567X",
                Title = "Test title",
                ShortTitle = "Test short title",
                PathId = 2,
                Source = "Test",
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
