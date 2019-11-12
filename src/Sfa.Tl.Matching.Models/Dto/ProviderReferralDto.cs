using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderReferralDto
    {
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string ProviderName { get; set; }
        public string DisplayName { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string ProviderVenueName { get; set; }
        public string CustomisedProviderDisplayName =>
            ProviderDisplayExtensions.GetDisplayText(ProviderVenueName, Postcode, DisplayName);
    }
}