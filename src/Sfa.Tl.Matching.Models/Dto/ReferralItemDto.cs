namespace Sfa.Tl.Matching.Models.Dto
{
    public class ReferralItemDto
    {
        public string Workplace { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string ProviderName { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string ProviderVenueTownAndPostcode { get; set; }
        public decimal DistanceFromEmployer { get; set; }

        public string PlacementsDetail =>
            PlacementsKnown.GetValueOrDefault()
                ? Placements.ToString()
                : "at least 1";
    }
}