using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
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

        public async Task PushProviderQuarterlyRequestMessageAsync(SendProviderQuarterlyUpdateEmail providerRequest)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(providerRequest),
                QueueName.ProviderQuarterlyRequestQueue);
        }

        public async Task PushEmployerReferralEmailMessageAsync(SendEmployerReferralEmail employerReferralEmail)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(employerReferralEmail),
                QueueName.EmployerReferralEmailQueue);
        }

        public async Task PushProviderReferralEmailMessageAsync(SendProviderReferralEmail providerReferralEmail)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(providerReferralEmail),
                QueueName.ProviderReferralEmailQueue);
        }

        public async Task PushEmployerAupaBlankEmailMessageAsync(SendEmployerAupaBlankEmail employerAupaBlankEmail)
        {
            await PushMessageAsync(
                JsonConvert.SerializeObject(employerAupaBlankEmail),
                QueueName.EmployerAupaBlankEmailQueue);
        }

        public async Task PushEmailDeliveryStatusMessageAsync(SendEmailDeliveryStatus emailDeliveryStatus)
        {
            await PushMessageAsync(JsonConvert.SerializeObject(emailDeliveryStatus),
                QueueName.EmailDeliveryStatusQueue);
        }

        private async Task PushMessageAsync(string message, string queueName)
        {
            var queue = await GetQueueAsync(queueName);
            await queue.AddMessageAsync(new CloudQueueMessage(message));
            _logger.LogInformation($"Added Message to Message Queue {queueName} => {message}");
        }

        private async Task<CloudQueue> GetQueueAsync(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudQueueClient();

            var queueReference = client.GetQueueReference(queueName);
            await queueReference.CreateIfNotExistsAsync();

            return queueReference;
        }
    }
}
