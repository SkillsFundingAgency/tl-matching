using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmailHistory : BaseEntity
    {
        public int ReferralId { get; set; }
        public int EmailTemplateId { get; set; }
        public string SentTo { get; set; }
        public string CopiedTo { get; set; }
        public string BlindCopiedTo { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; }
        public virtual Referral Referral { get; set; }
        public virtual ICollection<EmailPlaceholder> EmailPlaceholder { get; set; }
    }
}