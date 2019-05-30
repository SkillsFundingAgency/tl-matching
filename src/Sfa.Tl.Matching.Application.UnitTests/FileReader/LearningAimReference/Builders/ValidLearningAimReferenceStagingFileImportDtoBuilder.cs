using System;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Builders
{
    public class ValidLearningAimReferenceStagingFileImportDtoBuilder
    {
        public const string Title = "LearningAimReference";
        public const string LarId = "12345678";
        public const string AwardOrgLarId = "12345678";
        public static DateTime SourceCreatedOn = new DateTime(2019, 1, 1);
        public static DateTime SourceModifiedOn = new DateTime(2019, 1, 1);
        public const string CreatedBy = "CreatedBy";

        public LearningAimReferenceStagingFileImportDto Build() => new LearningAimReferenceStagingFileImportDto
        {
            Title = "LearningAimReference",
            LarId = "12345678",
            AwardOrgLarId = "12345678",
            SourceCreatedOn = "01 Jan 2019",
            SourceModifiedOn = "01 Jan 2019",
            CreatedBy = CreatedBy
        };
    }
}