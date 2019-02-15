using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Application.UnitTests.Data.ProviderVenue.Builders
{
    internal class ValidProviderVenueBuilder
    {
        private readonly Domain.Models.ProviderVenue _providerVenue;

        public ValidProviderVenueBuilder()
        {
            _providerVenue = new Domain.Models.ProviderVenue
            {
                Id = 1,
                ProviderId = 10000546,
                Postcode = "AA1 1AA",
                Town = "Town",
                County = "County",
                Status = true,
                StatusReason = "Reason",
                Latitude = 52.648869M,
                Longitude = -2.095574M,
                Source = "PMF_1018",
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
