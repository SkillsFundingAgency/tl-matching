using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.RoutePathMapping.Builders
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
                        Id = RoutePathMappingConstants.Id,
                        LarsId = RoutePathMappingConstants.LarsId,
                        Title = RoutePathMappingConstants.Title,
                        ShortTitle = RoutePathMappingConstants.ShortTitle,
                        PathId = RoutePathMappingConstants.PathId,
                        Source = RoutePathMappingConstants.Source,
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        ModifiedBy = EntityCreationConstants.ModifiedByUser,
                        ModifiedOn = EntityCreationConstants.ModifiedOn
                    },
                    new Domain.Models.RoutePathMapping
                    {
                        Id = RoutePathMappingConstants.Id2,
                        LarsId = RoutePathMappingConstants.LarsId2,
                        Title = RoutePathMappingConstants.Title2,
                        ShortTitle = RoutePathMappingConstants.ShortTitle2,
                        PathId = RoutePathMappingConstants.PathId2,
                        Source = RoutePathMappingConstants.Source,
                        CreatedBy = EntityCreationConstants.CreatedByUser,
                        CreatedOn = EntityCreationConstants.CreatedOn,
                        ModifiedBy = EntityCreationConstants.ModifiedByUser,
                        ModifiedOn = EntityCreationConstants.ModifiedOn
                    }
                };
        }

        public IEnumerable<Domain.Models.RoutePathMapping> Build() =>
            _routePathMappings;
    }
}
