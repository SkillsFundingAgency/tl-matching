using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailTemplate : BaseEntity
    {
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        public Guid TemplateUniqueId { get; set; }
        public string Recipients { get; set; }
    }
}
