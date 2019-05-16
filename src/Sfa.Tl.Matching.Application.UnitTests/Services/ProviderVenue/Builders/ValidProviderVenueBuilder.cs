namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders
{
    public class ValidProviderVenueBuilder
    {
        public Domain.Models.ProviderVenue Build() => new Domain.Models.ProviderVenue
        {
            Id = 1,
            Postcode = "CV1 2WT",
            Name = "Test Provider Venue",
            IsEnabledForReferral = true,
            IsRemoved = false,
            Source = "Test",
            CreatedBy = "CreatedBy",
            ModifiedBy = "ModifiedBy"
        };
    }
}
