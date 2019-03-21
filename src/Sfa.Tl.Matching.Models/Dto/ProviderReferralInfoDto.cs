using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderReferralInfoDto
    {
        public int ReferralId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPrimaryContact { get; set; }
        public string ProviderPrimaryContactEmail { get; set; }
        public string ProviderPrimaryContactPhone { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}
