using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Opportunity : BaseEntity
    {
        public int? EmployerId { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual ICollection<OpportunityItem> OpportunityItem { get; set; }
        public virtual ICollection<EmailHistory> EmailHistory { get; set; }
    }
}