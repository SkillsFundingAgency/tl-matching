using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Qualification : BaseEntity
    {
        public string LarId { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string QualificationSearch { get; set; }
        public string ShortTitleSearch { get; set; }
        
        public virtual ICollection<ProviderQualification> ProviderQualification { get; set; }
        public virtual ICollection<QualificationRouteMapping> QualificationRouteMapping { get; set; }
    }
}