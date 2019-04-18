namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class HideProviderVenueViewModel
    {
        public int ProviderVenueId { get; set; }
        public long UkPrn { get; set; }
        public string Postcode { get; set; }
        public string ProviderName { get; set; }
        public bool IsEnabledForSearch { get; set; }
    }
}