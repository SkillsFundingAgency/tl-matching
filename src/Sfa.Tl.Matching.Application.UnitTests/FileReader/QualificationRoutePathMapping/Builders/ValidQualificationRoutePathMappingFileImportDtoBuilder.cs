using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders
{
    public class ValidQualificationRoutePathMappingFileImportDtoBuilder
    {
        public const string LarsId = "1234567X";
        public const string Title = "Full Qualification Title";
        public const string ShortTitle = "Short Title";
        public const int PathId = 25;

        public const string Accountancy = "25";
        public const string AgricultureLandManagementandProduction = "1";
        public const string AnimalCareandManagement = "2";
        public const string Hospitality = "5";

        public QualificationRoutePathMappingFileImportDto Build() => new QualificationRoutePathMappingFileImportDto
        {
            LarsId = LarsId,
            Title = Title,
            ShortTitle = ShortTitle,
            Accountancy = Accountancy,
            Source = "Manual"
        };
    }
}