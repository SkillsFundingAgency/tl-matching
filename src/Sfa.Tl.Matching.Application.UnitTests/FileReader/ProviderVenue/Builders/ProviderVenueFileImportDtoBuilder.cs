using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders
{
    internal class ProviderVenueFileImportDtoBuilder
    {
        private readonly ProviderVenueFileImportDto _providerVenueFileImportDto;

        public ProviderVenueFileImportDtoBuilder()
        {
            _providerVenueFileImportDto = new ProviderVenueFileImportDto
            {
                PostCode = "AB1 1AA",
                ProviderId = 1,
                UkPrn = "10000546",
                Source = "PMF_1018"
            };
        }

        public ProviderVenueFileImportDto Build() =>
            _providerVenueFileImportDto;
    }
}