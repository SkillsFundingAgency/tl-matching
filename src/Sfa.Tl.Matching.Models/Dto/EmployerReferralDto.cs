using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerReferralDto
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactEmail { get; set; }
        public string JobTitle { get; set; }
        public string Postcode { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string RouteName { get; set; }
        public IEnumerable<ProviderReferralInfoDto> ProviderReferralInfo { get; set; }
        public string CreatedBy { get; set; }
    }
}
