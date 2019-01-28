using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class RoutePath
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public int RoutePathLookupId { get; set; }
        public string Summary { get; set; }
        public string Keywords { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<IndustryPlacement> IndustryPlacement { get; set; }
        public virtual Path Path { get; set; }
    }
}
