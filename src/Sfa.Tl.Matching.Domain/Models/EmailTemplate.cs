using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailTemplate : BaseEntity
    {
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        public virtual ICollection<EmailHistory> EmailHistory { get; set; }
    }
}
