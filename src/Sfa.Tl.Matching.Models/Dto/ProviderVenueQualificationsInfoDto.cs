using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationsInfoDto
    {
        public string Postcode { get; set; }

        public IEnumerable<QualificationInfoDto> Qualifications { get; set; }
    }
}