using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Parsing
{
    public class ProviderVenueParsingFixture
    {
        public ProviderVenueFileImportDto Dto;
        public ProviderVenueDataParser Parser;

        public ProviderVenueParsingFixture()
        {
            Dto = new ValidProviderVenueFileImportDtoBuilder().Build();
            Parser = new ProviderVenueDataParser();
        }
    }
}