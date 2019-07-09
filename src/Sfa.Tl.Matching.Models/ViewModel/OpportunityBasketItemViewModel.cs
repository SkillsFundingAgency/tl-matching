namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class OpportunityBasketItemViewModel
    {
        public int OpportunityItemId { get; set; }
        public bool IsSelected { get; set; }
        public string Workplace { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string OpportunityType { get; set; }

        public string PlacementsDetail =>
            Placements.GetValueOrDefault() > 1
                ? Placements.ToString()
                : "at least 1";
    }
}