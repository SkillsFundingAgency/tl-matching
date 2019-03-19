namespace Sfa.Tl.Matching.Models.Dto
{
    public class CheckAnswersDto : BaseOpportunityUpdateDto
    {
        public bool? ConfirmationSelected { get; set; }
        public string EmployerName { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string EmployerContact { get; set; }
    }
}