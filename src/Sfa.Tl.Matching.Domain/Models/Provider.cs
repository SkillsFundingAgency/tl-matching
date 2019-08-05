using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Provider : BaseEntity
    {
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int OfstedRating { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public bool IsCdfProvider { get; set; }
        public bool IsTLevelProvider { get; set; }
        public string Source { get; set; }
        public virtual ICollection<ProviderVenue> ProviderVenue { get; set; }
    }
}