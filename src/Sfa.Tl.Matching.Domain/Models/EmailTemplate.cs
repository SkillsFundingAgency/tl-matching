using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailTemplate
    {
        [Key]
        public Guid Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<NotificationHistory> NotificationHistory { get; set; }
        public virtual ICollection<TemplatePlaceholder> TemplatePlaceholder { get; set; }
    }
}