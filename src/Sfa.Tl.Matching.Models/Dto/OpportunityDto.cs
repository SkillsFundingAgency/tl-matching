using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public Guid EmployerCrmId { get; set; }
        public string PrimaryContact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public int OpportunityItemCount { get; set; }
    }
}