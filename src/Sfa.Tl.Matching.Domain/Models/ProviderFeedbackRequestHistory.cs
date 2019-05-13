
namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderFeedbackRequestHistory : BaseEntity
    {
        public int ProviderCount { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
    }
}
