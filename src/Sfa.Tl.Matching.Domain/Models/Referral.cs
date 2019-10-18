using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Referral : BaseEntity
    {
        public int OpportunityItemId { get; set; }
        public int ProviderVenueId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DistanceFromEmployer { get; set; }
        public int? JourneyTimeByCar { get; set; }
        public int? JourneyTimeByPublicTransport { get; set; }
        public virtual OpportunityItem OpportunityItem { get; set; }
        public virtual ProviderVenue ProviderVenue { get; set; }
    }
}