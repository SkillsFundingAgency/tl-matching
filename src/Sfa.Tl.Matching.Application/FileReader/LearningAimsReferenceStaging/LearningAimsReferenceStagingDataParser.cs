using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.LearningAimsReferenceStaging
{
    public class LearningAimsReferenceStagingDataParser : IDataParser<LearningAimsReferenceStagingDto>
    {
        public IEnumerable<LearningAimsReferenceStagingDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is LearningAimsReferenceStagingFileImportDto data)) return null;

            var learningAimsReferenceDto = new LearningAimsReferenceStagingDto
            {
                LarId = data.LarId,
                Title = data.Title,
                AwardOrgLarId = data.AwardOrgLarId,
                SourceCreatedOn = data.SourceCreatedOn.ToDateTime(true),
                SourceModifiedOn = data.SourceModifiedOn.ToDateTime(true),
                CreatedBy = data.CreatedBy
            };

            return new List<LearningAimsReferenceStagingDto> { learningAimsReferenceDto };
        }
    }
}