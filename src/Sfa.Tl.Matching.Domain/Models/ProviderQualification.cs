namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderQualification : BaseEntity
    {
        public int ProviderVenueId { get; set; }
        public int QualificationId { get; set; }
        public string Source { get; set; }
        public virtual ProviderVenue ProviderVenue { get; set; }
        public virtual Qualification Qualification { get; set; }
        public bool IsDeleted { get; set; }
    }
}