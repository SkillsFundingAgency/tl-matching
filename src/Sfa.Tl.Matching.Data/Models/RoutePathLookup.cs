using System;

namespace Sfa.Tl.Matching.Data.Models
{
    public class RoutePathLookup
    {
        public int Id { get; set; }
        public string Route { get; set; }
        public string Path { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
