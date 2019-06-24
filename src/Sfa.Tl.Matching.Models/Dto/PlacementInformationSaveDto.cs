using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PlacementInformationSaveDto : BaseOpportunityUpdateDto
    {
        public int RouteId { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int SearchResultProviderCount { get; set; }
        public OpportunityType OpportunityType { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public bool NoSuitableStudent { get; set; }
        public bool HadBadExperience { get; set; }
        public bool ProvidersTooFarAway { get; set; }
    }
}