using SFA.DAS.Notifications.Api.Client.Configuration;

namespace Sfa.Tl.Matching.Application.Configuration
{
    public class MatchingConfiguration
    {
        public AuthenticationConfig Authentication { get; set; }

        public AzureSearchConfiguration AzureSearchConfiguration { get; set; }

        public string BlobStorageConnectionString { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        public string PostcodeRetrieverBaseUrl { get; set; }

        public string SqlConnectionString { get; set; }
    }
}