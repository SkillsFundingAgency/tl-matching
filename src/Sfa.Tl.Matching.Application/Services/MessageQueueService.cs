using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly ILogger<MessageQueueService> _logger;
        private readonly MatchingConfiguration _configuration;

        public MessageQueueService(ILogger<MessageQueueService> logger, MatchingConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task PushProviderQuarterlyRequestMessageAsync(SendProviderFeedbackEmail providerRequest)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(providerRequest),
                QueueName.ProviderQuarterlyRequestQueue);
        }

        public async Task PushEmployerReferralEmailMessageAsync(SendEmployerReferralEmail employerReferralEmail)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(employerReferralEmail),
                QueueName.ProviderQuarterlyRequestQueue);
        }

        public async Task PushProviderReferralEmailMessageAsync(SendProviderReferralEmail providerReferralEmail)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(providerReferralEmail),
                QueueName.ProviderQuarterlyRequestQueue);
        }

        private async Task PushMessageAsync(string message, string queueName)
        {
            var queue = await GetQueue(queueName);
            await queue.AddMessageAsync(new CloudQueueMessage(message));
            _logger.LogInformation($"Added Message to Message Queue {queueName} => {message}");
        }

        private async Task<CloudQueue> GetQueue(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudQueueClient();

            var queueReference = client.GetQueueReference(queueName);
            await queueReference.CreateIfNotExistsAsync();

            return queueReference;
        }
    }
}
