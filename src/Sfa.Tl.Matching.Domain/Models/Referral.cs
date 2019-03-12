using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Referral : BaseEntity
    {
        public int OpportunityId { get; set; }
        public int ProviderVenueId { get; set; }
        public decimal DistanceFromEmployer { get; set; }
        public virtual Opportunity Opportunity { get; set; }
        public virtual ProviderVenue ProviderVenue { get; set; }
        public virtual ICollection<EmailHistory> EmailHistory { get; set; }
    }
}