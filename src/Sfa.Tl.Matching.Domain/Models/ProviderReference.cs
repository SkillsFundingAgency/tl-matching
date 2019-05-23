using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderReference : BaseEntity
    {
        public long UkPrn { get; set; }
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed),]
        public int ChecksumCol { get; set; }
    }
}