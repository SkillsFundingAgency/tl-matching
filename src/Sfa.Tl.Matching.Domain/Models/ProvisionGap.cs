namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProvisionGap : BaseEntity
    {
        public int OpportunityItemId { get; set; }
        public bool? NoSuitableStudent { get; set; }
        public bool? HadBadExperience { get; set; }
        public bool? ProviderTooFarAway { get; set; }
        public virtual OpportunityItem OpportunityItem { get; set; }
    }
}