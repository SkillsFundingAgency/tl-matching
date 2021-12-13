using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Qualification.Builders
{
    public class ValidQualificationBuilder
    {
        public Domain.Models.Qualification Build() =>
            new()
            {
                Id = 1,
                LarId = "1000",
                Title = "Title",
                ShortTitle = "ShortTitle",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
    }
}
