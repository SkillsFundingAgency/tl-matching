using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup.Builders
{
    public class ValidPostcodeLookupListBuilder
    {
        public IList<Domain.Models.PostcodeLookup> Build() => new List<Domain.Models.PostcodeLookup>
        {
            new()
            {
                Id = 1,
                Postcode = "CV1 2WT",
                PrimaryLepCode = "E37000012",
                SecondaryLepCode = "E37000013",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new()
            {
                Id = 2,
                Postcode = "OX2 9EG",
                PrimaryLepCode = "E37000027",
                SecondaryLepCode = "E37000028",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
