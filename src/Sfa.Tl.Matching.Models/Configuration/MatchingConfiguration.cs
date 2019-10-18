
using System;

namespace Sfa.Tl.Matching.Models.Configuration
{
    public class MatchingConfiguration
    {
        public AuthenticationConfig Authentication { get; set; }
        public string AuthorisedAdminUserEmail { get; set; }
        public string BlobStorageConnectionString { get; set; }
        public string GovNotifyApiKey { get; set; }
        public Guid EmailDeliveryStatusToken { get; set; }
        public bool SendEmailEnabled { get; set; }
        public string PostcodeRetrieverBaseUrl { get; set; }
        public string SqlConnectionString { get; set; }
        public string GoogleMapsApiBaseUrl { get; set; }
        public string GoogleMapsApiKey { get; set; }
        public string CalendarJsonUrl { get; set; }
        public string EmployerFeedbackTimeSpan { get; set; }
        public string MatchingServiceSupportEmailAddress { get; set; }
    }
}