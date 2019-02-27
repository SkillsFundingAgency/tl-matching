using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders
{
    public class ValidPathBuilder
    {
        public Domain.Models.Path Build() => new Domain.Models.Path
        {
            Id = PathConstants.Id,
            Name = PathConstants.Name,
            Keywords = PathConstants.Keywords,
            Summary = PathConstants.Summary,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
