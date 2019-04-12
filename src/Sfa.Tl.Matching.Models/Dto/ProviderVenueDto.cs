namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueDto
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Source { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public bool IsEnabledForSearch { get; set; }
        public string CreatedBy { get; set; }
    }
}