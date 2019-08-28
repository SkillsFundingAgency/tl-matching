using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ReferralsViewModel
    {
        public int ReferralId { get; set; }
        public string Name { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }
        public string Postcode { get; set; }
        public decimal DistanceFromEmployer { get; set; }
        public string DisplayText =>
            ProviderDisplayExtensions.GetDisplayText(ProviderVenueName, Postcode, ProviderDisplayName);
    }
}