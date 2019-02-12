using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders
{
    internal class ValidProviderVenueBuilder
    {
        private readonly ProviderVenueFileImportDto _providerVenue;

        public ValidProviderVenueBuilder()
        {
            _providerVenue = new ProviderVenueFileImportDto
            {
                UkPrn = "10000546",
                Source = "PMF_1018"
            };
        }

        public ProviderVenueFileImportDto Build() => _providerVenue;
    }
}