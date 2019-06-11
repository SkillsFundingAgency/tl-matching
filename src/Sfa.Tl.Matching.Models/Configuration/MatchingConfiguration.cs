using SFA.DAS.Notifications.Api.Client.Configuration;

namespace Sfa.Tl.Matching.Models.Configuration
{
    public class MatchingConfiguration
    {
        public AuthenticationConfig Authentication { get; set; }

        public string AuthorisedAdminUserEmail { get; set; }

        public string BlobStorageConnectionString { get; set; }

        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        public string NotificationsSystemId { get; set; }

        public bool SendEmailEnabled { get; set; }

        public string PostcodeRetrieverBaseUrl { get; set; }

        public string SqlConnectionString { get; set; }
    }
}