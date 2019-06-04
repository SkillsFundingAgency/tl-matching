using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class ProviderReference : BaseEntity
    {
        [MergeKey]
        public long UkPrn { get; set; }
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ChecksumCol { get; set; }
    }
}