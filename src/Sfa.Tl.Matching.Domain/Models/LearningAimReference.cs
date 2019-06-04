using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class LearningAimReference : BaseEntity
    {
        [MergeKey]
        public string LarId { get; set; } 
        public string Title { get; set; }
        public string AwardOrgLarId { get; set; }
        public DateTime SourceCreatedOn { get; set; }
        public DateTime SourceModifiedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ChecksumCol { get; set; }
    }
}
