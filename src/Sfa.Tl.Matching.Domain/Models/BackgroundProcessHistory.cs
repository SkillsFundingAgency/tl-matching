
namespace Sfa.Tl.Matching.Domain.Models
{
    public class BackgroundProcessHistory : BaseEntity
    {
        public int RecordCount { get; set; }
        public string ProcessType { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
    }
}