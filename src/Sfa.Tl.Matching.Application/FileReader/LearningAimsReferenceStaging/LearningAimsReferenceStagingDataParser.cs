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
                LearnAimRef = data.LearnAimRef,
                LearnAimRefTitle = data.LearnAimRefTitle,
                AwardOrgAimRef = data.AwardOrgAimRef,
                SourceCreatedOn = data.SourceCreatedOn.ToDateTime(),
                SourceModifiedOn = data.SourceModifiedOn.ToDateTime()
            };

            return new List<LearningAimsReferenceStagingDto> { learningAimsReferenceDto };
        }
    }
}