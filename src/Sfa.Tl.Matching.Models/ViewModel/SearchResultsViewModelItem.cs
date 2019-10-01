using System;
using System.Collections.Generic;
using Humanizer;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchResultsViewModelItem
    {
        public int ProviderVenueId { get; set; }
        public bool IsSelected { get; set; }
        public string ProviderVenueTown { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public double? Distance { get; set; }
        public string ProviderName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }
        public bool IsTLevelProvider { get; set; }

        public long? TravelTimeByDriving { get; set; }
        public string TravelTimeByDrivingDisplay =>
            TravelTimeByDriving.HasValue && TravelTimeByDriving.Value > 0
                ? TimeSpan.FromSeconds(TravelTimeByDriving.Value)
                    .Humanize()
                : null;

        public long? TravelTimeByPublicTransport { get; set; }
        public string TravelTimeByPublicTransportDisplay =>
            TravelTimeByPublicTransport.HasValue && TravelTimeByPublicTransport.Value > 0
                ? TimeSpan.FromSeconds(TravelTimeByPublicTransport.Value)
                    .Humanize()
                : null;

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}
