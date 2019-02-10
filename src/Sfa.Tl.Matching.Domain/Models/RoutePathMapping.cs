namespace Sfa.Tl.Matching.Domain.Models
{
    public class RoutePathMapping : BaseEntity
    {
        public string LarsId { get; set; }

        public string Title { get; set; }
        
        public string ShortTitle { get; set; }
        
        public int PathId { get; set; }

        public virtual Path Path { get; set; }
    }
}