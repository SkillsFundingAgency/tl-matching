
namespace Sfa.Tl.Matching.Infrastucture.Configuration
{
    public class MatchingConfiguration
    {
        public AuthenticationConfig Authentication { get; set; }

        public AzureSearchConfiguration AzureSearchConfiguration { get; set; }

        public string BlobStorageConnectionString { get; set; }

        public string ServiceBusConnectionString { get; set; }

        public string GovNotifyAPIKey { get; set; }

        public string PostcodeRetrieverBaseUrl { get; set; }

        public string SqlConnectionString { get; set; }
    }
}
