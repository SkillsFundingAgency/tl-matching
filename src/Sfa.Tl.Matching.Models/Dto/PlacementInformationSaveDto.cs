namespace Sfa.Tl.Matching.Models.Dto
{
    public class PlacementInformationSaveDto : BaseOpportunityUpdateDto
    {
        public int RouteId { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
    }
}