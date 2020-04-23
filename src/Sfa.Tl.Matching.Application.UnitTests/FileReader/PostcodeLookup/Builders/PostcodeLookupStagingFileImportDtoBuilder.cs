using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Builders
{
    public class ValidPostcodeLookupStagingFileImportDtoBuilder
    {
        public PostcodeLookupStagingFileImportDto Build() => new PostcodeLookupStagingFileImportDto
        {
            Postcode = "CA1 1AA",
            LepCode = "E37000007",
            CreatedBy = "CreatedBy"
        };
    }
}