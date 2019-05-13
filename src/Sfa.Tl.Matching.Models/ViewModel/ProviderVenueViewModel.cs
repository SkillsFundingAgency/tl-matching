namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderVenueViewModel
    {
        public string Postcode { get; set; }
        public int QualificationCount { get; set; }
        public int ProviderVenueId { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsEnabledForReferral { get; set; }
    }
}