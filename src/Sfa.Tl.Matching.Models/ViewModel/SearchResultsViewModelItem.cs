using System.Collections.Generic;

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
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}
