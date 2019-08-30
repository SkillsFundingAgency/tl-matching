using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderReferralDto
    {
        public string ProviderVenueTown { get; set; }
        public string ProviderVenuePostCode { get; set; }
        public string ProviderName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderPrimaryContact { get; set; }
        public string ProviderPrimaryContactEmail { get; set; }
        public string ProviderPrimaryContactPhone { get; set; }
        public string ProviderSecondaryContact { get; set; }
        public string ProviderSecondaryContactEmail { get; set; }
        public string ProviderSecondaryContactPhone { get; set; }
        //primary contact full name] (Telephone: [primary contact telephone number]); Email: [primary contact email address]) 
        //[secondary contact full name] (Telephone: [secondary contact telephone number]); Email: [secondary contact email address]
        public string ProviderVenueName { get; set; }

        public string CustomisedProviderDisplayName =>
            ProviderDisplayExtensions.GetDisplayText(ProviderVenueName, ProviderVenuePostCode, ProviderDisplayName);
    }
}