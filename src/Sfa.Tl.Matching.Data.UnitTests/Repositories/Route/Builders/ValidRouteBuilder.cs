using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders
{
    public class ValidRouteBuilder
    {
        public Domain.Models.Route Build() => new Domain.Models.Route
        {
            Id = RouteConstants.Id,
            Name = RouteConstants.Name,
            Keywords = RouteConstants.Keywords,
            Summary = RouteConstants.Summary,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
