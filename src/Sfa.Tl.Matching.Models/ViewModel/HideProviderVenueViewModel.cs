namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class HideProviderVenueViewModel
    {
        public int ProviderId { get; set; }
        public int ProviderVenueId { get; set; }
        public string Postcode { get; set; }
        public string ProviderVenueName { get; set; }
        public bool IsEnabledForSearch { get; set; }
    }
}