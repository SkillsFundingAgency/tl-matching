namespace Sfa.Tl.Matching.Application.UnitTests.ProviderVenue.Builders
{
    internal class ValidProviderVenueBuilder
    {
        public static Domain.Models.ProviderVenue Build() => new Domain.Models.ProviderVenue
        {
            Id = 1,
        };
    }
}