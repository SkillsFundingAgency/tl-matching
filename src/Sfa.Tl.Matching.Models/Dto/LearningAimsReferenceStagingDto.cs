using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class LearningAimsReferenceStagingDto
    {
        public string LearnAimRef { get; set; }
        public string LearnAimRefTitle { get; set; }
        public string AwardOrgAimRef { get; set; }
        public DateTime SourceCreatedOn { get; set; }
        public DateTime SourceModifiedOn { get; set; }
    }
}