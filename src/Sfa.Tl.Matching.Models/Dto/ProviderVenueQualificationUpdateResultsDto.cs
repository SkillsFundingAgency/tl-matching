
namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationUpdateResultsDto
    {
        public long UkPrn { get; set; }
        public string VenuePostcode { get; set; }
        public string LarId { get; set; }
        public bool HasErrors { get; set; }
        public string Message { get; set; }
    }
}