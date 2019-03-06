namespace Sfa.Tl.Matching.Domain.Models
{
    public class Referral : BaseEntity
    {
        public int OpportunityId { get; set; }
        public virtual Opportunity Opportunity { get; set; }
        public int? EmailId { get; set; }
        public bool EmailSent { get; set; }
        public int TotalProviders { get; set; }
    }
}