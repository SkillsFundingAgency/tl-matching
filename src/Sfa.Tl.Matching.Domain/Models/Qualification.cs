using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Qualification : BaseEntity
    {
        public string LarsId { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public virtual ICollection<ProviderQualification> ProviderQualification { get; set; }
        public virtual ICollection<QualificationRoutePathMapping> QualificationRoutePathMapping { get; set; }
    }
}