using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Constants;
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
                UkPrn = ProviderVenueConstants.UkPrn,
                PostCode = ProviderVenueConstants.PostCode,
                ProviderId = ProviderVenueConstants.ProviderId,
                Source = ProviderVenueConstants.Source,
                CreatedBy = ProviderVenueConstants.CreatedBy
            };
        }

        public ProviderVenueFileImportDto Build() =>
            _providerVenueFileImportDto;
    }
}