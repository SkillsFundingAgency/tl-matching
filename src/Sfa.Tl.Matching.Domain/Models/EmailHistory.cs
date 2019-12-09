using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailHistory : BaseEntity
    {
        public Guid? NotificationId { get; set; }
        public int? OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public int EmailTemplateId { get; set; }
        public string SentTo { get; set; }
        public string CopiedTo { get; set; }
        public string BlindCopiedTo { get; set; }
        public string Status { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; }
        public virtual Opportunity Opportunity { get; set; }
        public virtual ICollection<EmailPlaceholder> EmailPlaceholder { get; set; }
    }
}