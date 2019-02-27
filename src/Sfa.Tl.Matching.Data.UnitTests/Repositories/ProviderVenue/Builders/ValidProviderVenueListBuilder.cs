using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderVenue.Builders
{
    public class ValidProviderVenueListBuilder
    {
        public IList<Domain.Models.ProviderVenue> Build() => new List<Domain.Models.ProviderVenue>
        { 
            new Domain.Models.ProviderVenue
            {
                Id = 1,
                ProviderId = 10000546,
                Postcode = "AA1 1AA",
                Town = "Town",
                County = "County",
                Latitude = 52.648869M,
                Longitude = -2.095574M,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.ProviderVenue
            {
                Id = 2,
                ProviderId = 10000123,
                Postcode = "AA2 2AA",
                Town = "Town2",
                County = "County2",
                Latitude = 50.648869M,
                Longitude = -1.095574M,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
