namespace Sfa.Tl.Matching.Domain.Models
{
    public class OpportunityBasketItem
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string OpportunityType { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameAka { get; set; }
        public string JobRole { get; set; }
        public int Placements { get; set; }
        public bool? PlacementsKnown { get; set; }
        public string Workplace { get; set; }
        public bool? HadBadExperience { get; set; }
        public bool? NoSuitableStudent { get; set; }
        public bool? ProvidersTooFarAway { get; set; }
        public string DisplayName { get; set; }
        public string VenueName { get; set; }
        public string Postcode { get; set; }
    }
}
