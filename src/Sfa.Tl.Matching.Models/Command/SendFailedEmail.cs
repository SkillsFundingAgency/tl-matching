using System;

namespace Sfa.Tl.Matching.Models.Command
{
    public class SendFailedEmail
    {
        public Guid NotificationId { get; set; }
    }
}