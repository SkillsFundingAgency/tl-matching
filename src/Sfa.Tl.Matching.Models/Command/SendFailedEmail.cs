using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Command
{
    public class SendFailedEmail
    {
        public string NotificationId { get; set; }
        public EmailType EmailType { get; set; }
    }
}