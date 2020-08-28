using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    // ReSharper disable UnusedMember.Global
    public class Opportunity : BaseEntity
    {
        public Guid? EmployerCrmId { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual ICollection<OpportunityItem> OpportunityItem { get; set; }
        public virtual ICollection<EmailHistory> EmailHistory { get; set; }
    }
}