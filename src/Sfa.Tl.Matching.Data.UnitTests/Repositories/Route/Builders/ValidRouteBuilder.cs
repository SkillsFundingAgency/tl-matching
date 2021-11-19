using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Route.Builders
{
    public class ValidRouteBuilder
    {
        public Domain.Models.Route Build() => new()
        {
            Id = 1,
            Name = "Route 1",
            Keywords = "Keyword",
            Summary = "Route Summary",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
