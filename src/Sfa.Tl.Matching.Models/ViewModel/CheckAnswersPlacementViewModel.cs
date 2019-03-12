namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersPlacementViewModel
    {
        public string EmployerName { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short Distance { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string Contact { get; set; }
    }
}