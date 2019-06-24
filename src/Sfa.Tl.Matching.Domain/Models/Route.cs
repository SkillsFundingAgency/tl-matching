using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable once UnusedMember.Global
    public class Route : BaseEntity
    {
        public string Name { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public virtual ICollection<OpportunityItem> OpportunityItem { get; set; }
        public virtual ICollection<Path> Path { get; set; }
        public virtual ICollection<QualificationRoutePathMapping> QualificationRoutePathMapping { get; set; }
    }
}