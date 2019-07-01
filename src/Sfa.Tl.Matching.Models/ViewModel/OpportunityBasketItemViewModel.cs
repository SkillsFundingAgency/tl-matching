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

        public string StudentsWanted =>
            PlacementsKnown.GetValueOrDefault()
                ? Placements.ToString()
                : "at least one";
    }
}