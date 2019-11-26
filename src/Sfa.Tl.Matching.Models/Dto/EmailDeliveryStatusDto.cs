using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmailDeliveryStatusDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public EmailDeliveryStatusType EmailDeliveryStatusType { get; set; }
    }
}