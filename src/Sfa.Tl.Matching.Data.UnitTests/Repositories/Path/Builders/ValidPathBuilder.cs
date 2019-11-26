using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders
{
    public class ValidPathBuilder
    {
        public Domain.Models.Path Build() => new Domain.Models.Path
        {
            Id = 1,
            Name = "Path 1",
            Keywords = "Keyword",
            Summary = "Path summary",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
