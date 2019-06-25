using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityDto
    {
        public int Id { get; set; }
        public int? EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<OpportunityItemDto> OpportunityItem { get; set; }
        public int OpportunityItemCount { get; set; }
    }
}