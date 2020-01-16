using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximityReportItemDto
    {
        public int ProviderVenueId { get; set; }
        public string ProviderVenueTown { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public double? Distance { get; set; }
        public string ProviderName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }
        public bool IsTLevelProvider { get; set; }

        public long? TravelTimeByDriving { get; set; }
        public long? TravelTimeByPublicTransport { get; set; }
        
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public IEnumerable<RouteAndQualificationsViewModel> Routes { get; set; }

        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }

    }
}