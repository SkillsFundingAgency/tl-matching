using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Path : BaseEntity
    {
        public int RouteId { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
        public string Summary { get; set; }

        public virtual Route Route { get; set; }
    }
}