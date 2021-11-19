using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximitySearchResultViewModelItem
    {
        public ProviderProximitySearchResultViewModelItem()
        {
        }

        public ProviderProximitySearchResultViewModelItem(int providerVenueId, string providerName, string providerVenuePostcode, double? distance, IEnumerable<RouteAndQualificationsViewModel> routes)
        {
            ProviderVenueId = providerVenueId;
            ProviderName = providerName;
            ProviderVenuePostcode = providerVenuePostcode;
            Distance = distance;
            Routes = routes;
        }

        public int ProviderVenueId { get; set; }
        public string ProviderVenueTown { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public double? Distance { get; set; }
        public string ProviderName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }
        public bool IsTLevelProvider { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public IEnumerable<RouteAndQualificationsViewModel> Routes { get; set; }
    }
}