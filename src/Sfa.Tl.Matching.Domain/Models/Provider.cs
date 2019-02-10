using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Provider : BaseEntity
    {
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public int OfstedRating { get; set; }
        public bool Status { get; set; }
        public string StatusReason { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string Source { get; set; }
        
        public virtual ICollection<ProviderVenue> ProviderVenue { get; set; }
    }
}