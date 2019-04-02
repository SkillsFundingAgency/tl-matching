using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerNameDto : BaseOpportunityUpdateDto
    {
        public int EmployerId { get; set; }
        public string EmployerName { get; set; }
        public Guid EmployerCrmId { get; set; }
    }
}