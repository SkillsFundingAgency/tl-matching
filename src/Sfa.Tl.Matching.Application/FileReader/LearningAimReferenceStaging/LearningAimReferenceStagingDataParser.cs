using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging
{
    public class LearningAimReferenceStagingDataParser : IDataParser<LearningAimReferenceStagingDto>
    {
        public IEnumerable<LearningAimReferenceStagingDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is LearningAimReferenceStagingFileImportDto data)) return null;

            var learningAimReferenceStagingDto = new LearningAimReferenceStagingDto
            {
                LarId = data.LarId,
                Title = data.Title,
                AwardOrgLarId = data.AwardOrgLarId,
                SourceCreatedOn = data.SourceCreatedOn.ToDateTime(),
                SourceModifiedOn = data.SourceModifiedOn.ToDateTime(),
                CreatedBy = data.CreatedBy
            };

            return new List<LearningAimReferenceStagingDto> { learningAimReferenceStagingDto };
        }
    }
}