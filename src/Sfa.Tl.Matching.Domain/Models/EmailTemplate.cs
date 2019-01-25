using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<NotificationHistory> NotificationHistory { get; set; }
        public virtual ICollection<TemplatePlaceholder> TemplatePlaceholder { get; set; }
    }
}
