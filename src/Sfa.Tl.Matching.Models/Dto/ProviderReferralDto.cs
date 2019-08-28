using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class WorkplaceDto
    {
        public string  WorkplaceTown { get; set; }
        public string WorkplacePostcode { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public IEnumerable<ProviderReferralDto> ProviderAndVenueDetails { get; set; }

    }

    public class ProviderReferralDto
    {
        public string ProviderVenueTown { get; set; }
        public string ProviderVenuePostCode { get; set; }
        public string ProviderName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }

        public string CustomisedProviderDisplayName =>
            ProviderDisplayExtensions.GetDisplayText(ProviderVenueName, ProviderVenuePostCode, ProviderDisplayName);
    }
}