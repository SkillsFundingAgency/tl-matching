using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders
{
    public class ValidProviderVenueBuilder
    {
        public Domain.Models.ProviderVenue Build() => new()
        {
                Id = 1,
                ProviderId = 10000546,
                Postcode = "AA1 1AA",
                Name = "Venue Name",
                Town = "Town",
                County = "County",
                Latitude = 52.648869M,
                Longitude = -2.095574M,
                IsEnabledForReferral = true,
                IsRemoved = false,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
    }
}
