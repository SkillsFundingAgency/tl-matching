using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders
{
    internal class ProviderVenueFileImportDtoBuilder
    {
        public static ProviderVenueFileImportDto Build() => new ProviderVenueFileImportDto
        {
            PostCode = "AB1 1AA",
            ProviderId = 1,
            UkPrn = "10000546",
            Source = "PMF_1018"
        };
    }
}