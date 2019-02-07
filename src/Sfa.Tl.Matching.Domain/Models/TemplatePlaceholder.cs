using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class TemplatePlaceholder
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EmailTemplateId { get; set; }
        public string Placeholder { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }
    }
}