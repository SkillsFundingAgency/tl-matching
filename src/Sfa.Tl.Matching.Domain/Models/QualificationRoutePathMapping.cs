namespace Sfa.Tl.Matching.Domain.Models
{
    public class QualificationRoutePathMapping : BaseEntity
    {
        public int RouteId { get; set; }
        public int QualificationId { get; set; }
        public string Source { get; set; }
        public virtual Route Route { get; set; }
        public virtual Qualification Qualification { get; set; }
    }
}