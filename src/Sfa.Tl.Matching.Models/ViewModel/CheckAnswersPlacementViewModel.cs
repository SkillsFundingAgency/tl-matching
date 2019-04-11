namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersPlacementViewModel
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int RouteId { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string EmployerContact { get; set; }
    }
}