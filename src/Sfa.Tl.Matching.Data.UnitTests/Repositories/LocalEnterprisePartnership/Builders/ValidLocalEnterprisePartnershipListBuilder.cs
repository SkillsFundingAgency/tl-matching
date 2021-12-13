using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.LocalEnterprisePartnership.Builders
{
    public class ValidLocalEnterprisePartnershipListBuilder
    {
        public IList<Domain.Models.LocalEnterprisePartnership> Build() => new List<Domain.Models.LocalEnterprisePartnership>
        {
            new()
            {
                Id = 1,
                Code = "E37000012",
                Name = "Greater Birmingham and Solihull",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new()
            {
                Id = 2,
                Code = "E37000027",
                Name = "Oxfordshire",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
