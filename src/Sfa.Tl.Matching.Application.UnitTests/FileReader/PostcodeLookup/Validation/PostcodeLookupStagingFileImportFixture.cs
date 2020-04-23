using Sfa.Tl.Matching.Application.FileReader.PostcodeLookupStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Validation
{
    public class PostcodeLookupStagingFileImportFixture
    {
        public PostcodeLookupStagingDataValidator Validator;
        public PostcodeLookupStagingFileImportDto Dto;

        public PostcodeLookupStagingFileImportFixture()
        {
            Dto = new ValidPostcodeLookupStagingFileImportDtoBuilder().Build();
            Validator = new PostcodeLookupStagingDataValidator();
        }
    }
}