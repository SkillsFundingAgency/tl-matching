using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.PostcodeLookup.Builders
{
    public class ValidPostcodeLookupListBuilder
    {
        public IList<Domain.Models.PostcodeLookup> Build() => new List<Domain.Models.PostcodeLookup>
        {
            new Domain.Models.PostcodeLookup
            {
                Id = 1,
                Postcode = "CV1 2WT",
                LepCode = "E37000012",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.PostcodeLookup
            {
                Id = 2,
                Postcode = "OX2 9EG",
                LepCode = "E37000027",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
