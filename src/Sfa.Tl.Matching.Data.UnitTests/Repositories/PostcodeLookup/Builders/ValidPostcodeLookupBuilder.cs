using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup.Builders
{
    public class ValidPostcodeLookupBuilder
    {
        public Domain.Models.PostcodeLookup Build() => new Domain.Models.PostcodeLookup
        {
            Id = 1,
            Postcode = "CV1 2WT",
            LepCode = "E37000012",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
