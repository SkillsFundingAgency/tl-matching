using System;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders
{
    public class ValidProviderVenueBuilder
    {
        public Domain.Models.ProviderVenue Build() => new Domain.Models.ProviderVenue
        {
            Id = 1,
            Postcode = "CV1 2WT",
            Name = "Test Provider Venue",
            IsEnabledForSearch = true,
            Source = "Test",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
