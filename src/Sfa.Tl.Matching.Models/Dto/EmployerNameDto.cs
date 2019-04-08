using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerNameDto : BaseOpportunityUpdateDto
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public bool HasChanged { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public Guid EmployerCrmId { get; set; }
    }
}