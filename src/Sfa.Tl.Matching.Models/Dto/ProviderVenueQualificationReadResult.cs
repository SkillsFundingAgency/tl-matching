using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationReadResultDto
    {
        public IList<ProviderVenueQualificationDto> ProviderVenueQualifications { get; set; }
        public string Error { get; set; }
    }
}