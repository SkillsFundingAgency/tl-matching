namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderSearchDto : BaseOpportunityUpdateDto
    {
        public int RouteId { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int? SearchResultProviderCount { get; set; }
    }
}