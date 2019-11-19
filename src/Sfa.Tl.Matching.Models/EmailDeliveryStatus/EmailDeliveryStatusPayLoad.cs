using System;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Models.EmailDeliveryStatus
{
    public class EmailDeliveryStatusPayLoad
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
        
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonProperty("sent_at")]
        public DateTime? SentAt { get; set; }

        [JsonProperty("notification_type")]
        public string NotificationType { get; set; }
        public string EmailDeliveryStatus => string.IsNullOrEmpty(status) ? "unknown-failure" : status;

    }
}
