using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders
{
    public class ValidProviderVenueFileImportDtoBuilder
    {
        public static int ProviderId = 1;
        public static string UkPrn = "10000546";
        public static string PostCode = "CV1 2WT";
        public static string Source = "PMF_1018";
        public static string CreatedBy = "CreatedBy";

        public ProviderVenueFileImportDto Build() => new ProviderVenueFileImportDto
        {
            ProviderId = ProviderId,
            UkPrn = UkPrn,
            PostCode = PostCode,
            Source = Source,
            CreatedBy = CreatedBy
        };

    }
}