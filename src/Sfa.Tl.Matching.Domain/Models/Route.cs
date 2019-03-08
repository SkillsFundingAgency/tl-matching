using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Route : BaseEntity
    {
        public string Name { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }
        public virtual ICollection<Opportunity> Opportunity { get; set; }
        public virtual ICollection<Path> Path { get; set; }
    }
}