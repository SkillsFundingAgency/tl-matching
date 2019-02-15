using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenue
{
    public class ProviderVenueDataParser : IDataParser<ProviderVenueDto>
    {
        public IEnumerable<ProviderVenueDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is ProviderVenueFileImportDto data)) return null;

            var providerVenueDto = new ProviderVenueDto
            {
                ProviderId = data.ProviderId,
                Postcode = data.PostCode,
                Source = data.Source,
                CreatedBy = data.CreatedBy
            };

            return new List<ProviderVenueDto> { providerVenueDto };
        }
    }
}