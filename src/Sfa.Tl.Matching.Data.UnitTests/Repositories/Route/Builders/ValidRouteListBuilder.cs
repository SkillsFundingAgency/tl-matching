using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders
{
    public class ValidRouteListBuilder
    {
        public IList<Domain.Models.Route> Build() => new List<Domain.Models.Route>
        {
            new()
            {
                Id = 1,
                Name = "Route 1",
                Keywords = "Keyword",
                Summary = "Route summary",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new()
            {
                Id = 2,
                Name = "Route 2",
                Keywords = "Keyword",
                Summary = "Route summary",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
