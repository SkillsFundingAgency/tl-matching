using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerReferralDto
    {
        public int OpportunityId { get; set; }
        public string CompanyName { get; set; }
        public string PrimaryContact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string RouteName { get; set; }
        public IEnumerable<WorkplaceDto> WorkplaceDetails { get; set; }
        public string CreatedBy { get; set; }
    }
}
