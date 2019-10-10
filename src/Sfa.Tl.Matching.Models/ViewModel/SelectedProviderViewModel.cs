namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SelectedProviderViewModel
    {
        public bool IsSelected { get; set; }
        public int ProviderVenueId { get; set; }
        public decimal? DistanceFromEmployer { get; set; }
        public int? JourneyTimeByCar { get; set; }
        public int? JourneyTimeByPublicTransport { get; set; }
    }
}