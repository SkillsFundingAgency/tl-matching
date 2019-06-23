namespace Sfa.Tl.Matching.Domain.Models
{
    public class Referral : BaseEntity
    {
        public int OpportunityItemId { get; set; }
        public int ProviderVenueId { get; set; }
        public decimal DistanceFromEmployer { get; set; }
        public virtual OpportunityItem OpportunityItem { get; set; }
        public virtual ProviderVenue ProviderVenue { get; set; }
    }
}