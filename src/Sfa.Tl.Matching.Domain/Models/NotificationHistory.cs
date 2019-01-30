using System;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Domain.Models
{
    public class NotificationHistory
    {
        public Guid Id { get; set; }
        public Guid EmailTemplateId { get; set; }
        public Guid EntityRefId { get; set; }
        public string Sender { get; set; }
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }
        public virtual Provider EntityRefNavigation { get; set; }
    }
}