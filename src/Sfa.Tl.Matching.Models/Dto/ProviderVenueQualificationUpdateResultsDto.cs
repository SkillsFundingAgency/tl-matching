using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationUpdateResultsDto
    {
        public long UkPrn { get; set; }
        public bool HasErrors { get; set; }
        public string Message { get; set; }
    }
}