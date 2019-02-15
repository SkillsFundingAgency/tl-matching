using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders
{
    internal class ValidProviderVenueBuilder
    {
        private readonly Domain.Models.ProviderVenue _providerVenue;

        public ValidProviderVenueBuilder()
        {
            _providerVenue = new Domain.Models.ProviderVenue
            {
                Id = 1,
                ProviderId = ProviderVenueConstants.ProviderId,
                Postcode = ProviderVenueConstants.Postcode,
                Town = ProviderVenueConstants.Town,
                County = ProviderVenueConstants.County,
                Status = ProviderVenueConstants.Status,
                StatusReason = ProviderVenueConstants.StatusReason,
                Latitude = ProviderVenueConstants.Latitude,
                Longitude = ProviderVenueConstants.Longitude,
                Source = ProviderVenueConstants.Source,
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public Domain.Models.ProviderVenue Build() =>
            _providerVenue;
    }
}
