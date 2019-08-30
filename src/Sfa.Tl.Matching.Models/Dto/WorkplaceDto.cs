using System.Collections.Generic;

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
}