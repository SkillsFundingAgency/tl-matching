using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class OpportunityItem : BaseEntity
    {
        public int OpportunityId { get; set; }
        public int RouteId { get; set; }
        public string OpportunityType { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public int? SearchResultProviderCount { get; set; }
        public bool IsSaved { get; set; }
        public bool IsSelectedForReferral { get; set; }
        public bool IsCompleted { get; set; }
        public bool EmployerFeedbackSent { get; set; }
        public virtual Opportunity Opportunity { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<ProvisionGap> ProvisionGap { get; set; }
        public virtual ICollection<Referral> Referral { get; set; }
    }
}