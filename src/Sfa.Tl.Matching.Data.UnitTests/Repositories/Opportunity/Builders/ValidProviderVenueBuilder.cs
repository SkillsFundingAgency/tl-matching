using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidProviderVenueBuilder
    {
        public Domain.Models.ProviderVenue Build() => new Domain.Models.ProviderVenue
        {
            Id = 1,
            ProviderId = 1,
            Postcode = "AA1 1AA",
            Town = "Town",
            County = "County",
            Latitude = 52.648869M,
            Longitude = 2.095574M,
            Source = "Test",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
