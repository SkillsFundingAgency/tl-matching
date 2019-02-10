using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderDto
    {
        public int Id { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public OfstedRating OfstedRating { get; set; }
        public bool Status { get; set; }
        public string StatusReason { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string Source { get; set; }
    }
}