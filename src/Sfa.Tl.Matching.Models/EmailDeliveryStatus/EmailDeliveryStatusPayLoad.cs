using System;
using System.Text.Json.Serialization;

namespace Sfa.Tl.Matching.Models.EmailDeliveryStatus
{
    public class EmailDeliveryStatusPayLoad
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }
        
        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonPropertyName("sent_at")]
        public DateTime? SentAt { get; set; }

        [JsonPropertyName("notification_type")]
        public string NotificationType { get; set; }
        public string EmailDeliveryStatus => string.IsNullOrEmpty(Status) ? "unknown-failure" : Status;

    }
}
