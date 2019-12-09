using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerFeedbackDto
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public Guid EmployerCrmId { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string JobRole { get; set; }
        public int? Placements { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}