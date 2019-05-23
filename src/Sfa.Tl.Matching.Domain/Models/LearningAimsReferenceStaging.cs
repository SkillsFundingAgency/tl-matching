using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class LearningAimsReferenceStaging : BaseEntity
    {
        public string LearnAimRef { get; set; } 
        public string LearnAimRefTitle { get; set; }
        public string AwardOrgAimRef { get; set; }
        public string SourceCreatedOn { get; set; }
        public string SourceModifiedOn { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ChecksumCol { get; set; }
    }
}
