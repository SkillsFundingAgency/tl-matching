using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ReferralItemDto
    {
        public string Workplace { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string ProviderVenueTownAndPostcode { get; set; }
        public string ProviderVenueName { get; set; }
        public decimal DistanceFromEmployer { get; set; }
        public int? JourneyTimeByCar { get; set; }
        public int? JourneyTimeByPublicTransport { get; set; }
        public string PlacementsDetail =>
            PlacementsKnown.GetValueOrDefault()
                ? Placements.ToString()
                : "At least 1";
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderName => ProviderDisplayExtensions.GetDisplayText(ProviderVenueName, Postcode, ProviderDisplayName);
    }
}