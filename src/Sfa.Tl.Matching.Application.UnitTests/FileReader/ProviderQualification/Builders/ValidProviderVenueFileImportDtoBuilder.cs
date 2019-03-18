using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Builders
{
    public class ValidProviderQualificationFileImportDtoBuilder
    {
        public static string UkPrn = "10000546";
        public static string Postcode = "CV1 2WT";
        public static string LarsId = "12345678";
        public static int NumberOfPlacements = 1;
        public static string Source = "PMF_1018";
        public static string CreatedBy = "CreatedBy";
        public static int QualificationId = 1;
        public static int ProviderVenueId = 1;

        public ProviderQualificationFileImportDto Build() => new ProviderQualificationFileImportDto
        {
            ProviderVenueId = ProviderVenueId,
            QualificationId = QualificationId,
            UkPrn = UkPrn,
            Postcode = Postcode,
            LarsId = LarsId,
            NumberOfPlacements = NumberOfPlacements.ToString(),
            Source = Source,
            CreatedBy = CreatedBy
        };

    }
}