using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class LearningAimsReference : BaseEntity
    {
        public string LarId { get; set; } 
        public string Title { get; set; }
        public string AwardOrgLarId { get; set; }
        public string SourceCreatedOn { get; set; }
        public string SourceModifiedOn { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed),]
        public int ChecksumCol { get; set; }
    }
}
