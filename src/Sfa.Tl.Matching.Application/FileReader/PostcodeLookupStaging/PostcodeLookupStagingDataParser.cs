using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.PostcodeLookupStaging
{
    public class PostcodeLookupStagingDataParser : IDataParser<PostcodeLookupStagingDto>
    {
        public IEnumerable<PostcodeLookupStagingDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is PostcodeLookupStagingFileImportDto data)) return null;

            var postcodeLookupStagingDto = new PostcodeLookupStagingDto
            {
                Postcode = data.Postcode,
                PrimaryLepCode = data.PrimaryLepCode,
                SecondaryLepCode = data.SecondaryLepCode,
                CreatedBy = data.CreatedBy
            };

            return new List<PostcodeLookupStagingDto> { postcodeLookupStagingDto };
        }
    }
}
