using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Builders
{
    public class ValidPostcodeLookupStagingFileImportDtoBuilder
    {
        public PostcodeLookupStagingFileImportDto Build() => new PostcodeLookupStagingFileImportDto
        {
            Postcode = "CA1 1AA",
            PrimaryLepCode = "E37000007",
            SecondaryLepCode = "E37000008",
            CreatedBy = "CreatedBy"
        };
    }
}