namespace Sfa.Tl.Matching.Domain.Models
{
    public class QualificationRoutePathMapping : BaseEntity
    {
        public int PathId { get; set; }
        public int QualificationId { get; set; }
        public string Source { get; set; }
        public virtual Path Path { get; set; }
        public virtual Qualification Qualification { get; set; }
    }
}