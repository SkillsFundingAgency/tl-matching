using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderDto
    {
        public int Id { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public OfstedRating OfstedRating { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public bool IsCdfProvider { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
    }
}