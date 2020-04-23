using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class PostcodeLookupStaging : BaseEntity
    {
        [MergeKey]
        public string Postcode { get; set; } 
        public string LepCode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ChecksumCol { get; set; }
    }
}
