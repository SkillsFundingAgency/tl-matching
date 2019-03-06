namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersGapViewModel
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }
        public string Route { get; set; }
        public string Postcode { get; set; }
        public short Distance { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string Contact { get; set; }
        public bool ConfirmationSelected { get; set; }
        public string CreatedBy { get; set; }
    }
}