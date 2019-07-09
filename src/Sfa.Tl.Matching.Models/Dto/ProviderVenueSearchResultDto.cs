using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueSearchResultDto
    {
        public int ProviderVenueId { get; set; }
        public string ProviderName { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public double? Distance { get; set; }
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}