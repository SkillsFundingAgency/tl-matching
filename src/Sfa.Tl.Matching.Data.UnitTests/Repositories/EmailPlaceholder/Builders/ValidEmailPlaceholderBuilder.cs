using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder.Builders
{
    public class ValidEmailPlaceholderBuilder
    {
        public Domain.Models.EmailPlaceholder Build() => new()
        {
            Id = 1,
            EmailHistoryId = 1,
            Key = "name_Placeholder",
            Value = "Name",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
