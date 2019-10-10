using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Command
{
    public class SendFailedEmail
    {
        public EmailType EmailType { get; set; }
    }
}