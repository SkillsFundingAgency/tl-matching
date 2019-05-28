using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class LearningAimsReferenceStagingDto
    {
        public string LarId { get; set; }
        public string Title { get; set; }
        public string AwardOrgLarId { get; set; }
        public DateTime SourceCreatedOn { get; set; }
        public DateTime SourceModifiedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}