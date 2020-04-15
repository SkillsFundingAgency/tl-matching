using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.LocalEnterprisePartnership.Builders
{
    public class ValidLocalEnterprisePartnershipBuilder
    {
        public Domain.Models.LocalEnterprisePartnership Build() => new Domain.Models.LocalEnterprisePartnership
        {
            Id = 1,
            Code = "E37000012",
            Name = "Greater Birmingham and Solihull",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
