using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.Models
{
    public class IndustryPlacement
    {
        public Guid Id { get; set; }
        public Guid? RoutePathId { get; set; }
        public Guid? AddressId { get; set; }
        public int? Placement { get; set; }
        public int? PlacementReferred { get; set; }
        public int? PlacementOffered { get; set; }
        public bool? PlacementGap { get; set; }
        public DateTime? ContactedOn { get; set; }
        public DateTime? NextContactOn { get; set; }
        public string Resolution { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Address Address { get; set; }
        public virtual RoutePath RoutePath { get; set; }
    }
}
