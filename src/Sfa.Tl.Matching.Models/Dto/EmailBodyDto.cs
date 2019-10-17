using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmailBodyDto
    {
        public string PrimaryContactEmail { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string EmployerEmail { get; set; }
        public string ProviderVenueName { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderName =>
            ProviderDisplayExtensions.GetProviderEmailDisplayText(ProviderVenueName, ProviderVenuePostcode,
                ProviderDisplayName);
    }
}