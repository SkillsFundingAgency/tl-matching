
namespace Sfa.Tl.Matching.Models.Dto
{
    public class QualificationRoutePathMappingDto
    {
        public QualificationDto Qualification { get; set; }
        public int PathId { get; set; }
        public int QualificationId { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
    }
}