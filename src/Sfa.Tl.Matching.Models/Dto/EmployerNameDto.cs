using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerNameDto
    {
        public int OpportunityId { get; set; }
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}