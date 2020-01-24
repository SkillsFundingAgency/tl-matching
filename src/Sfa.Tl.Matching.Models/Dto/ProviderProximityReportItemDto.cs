using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderProximityReportItemDto
    {
        public string ProviderVenueTown { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public double? Distance { get; set; }
        public string ProviderName => ProviderDisplayExtensions.GetDisplayText(ProviderVenueName, ProviderVenuePostcode, ProviderDisplayName);
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }
        
        public IEnumerable<RouteAndQualificationsDto> Routes { get; set; }

        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
    }
}