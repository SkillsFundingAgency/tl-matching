namespace Sfa.Tl.Matching.Models.Dto
{
    public class CheckAnswersDto : BaseOpportunityDto
    {
        public string EmployerName { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int RouteId { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public bool? IsSaved { get; set; }
    }
}