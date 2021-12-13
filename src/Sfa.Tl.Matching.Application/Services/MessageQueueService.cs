using System.Text.Json;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
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
                JsonSerializer.Serialize(providerRequest),
                QueueName.ProviderQuarterlyRequestQueue);
        }

        public async Task PushEmployerReferralEmailMessageAsync(SendEmployerReferralEmail employerReferralEmail)
        {
            await PushMessageAsync(
                JsonSerializer.Serialize(employerReferralEmail),
                QueueName.EmployerReferralEmailQueue);
        }

        public async Task PushProviderReferralEmailMessageAsync(SendProviderReferralEmail providerReferralEmail)
        {
            await PushMessageAsync(
                JsonSerializer.Serialize(providerReferralEmail),
                QueueName.ProviderReferralEmailQueue);
        }

        public async Task PushEmployerAupaBlankEmailMessageAsync(SendEmployerAupaBlankEmail employerAupaBlankEmail)
        {
            await PushMessageAsync(
                JsonSerializer.Serialize(employerAupaBlankEmail),
                QueueName.EmployerAupaBlankEmailQueue);
        }

        public async Task PushEmailDeliveryStatusMessageAsync(SendEmailDeliveryStatus emailDeliveryStatus)
        {
            await PushMessageAsync(JsonSerializer.Serialize(emailDeliveryStatus),
                QueueName.EmailDeliveryStatusQueue);
        }

        private async Task PushMessageAsync(string message, string queueName)
        {
            var queueClient = await CreateQueueClient(queueName);
            await queueClient.SendMessageAsync(message);

            _logger.LogInformation($"Added Message to Message Queue {queueName} => {message}");
        }

        private async Task<QueueClient> CreateQueueClient(string queueName)
        {
            var queueClient = new QueueClient(_configuration.BlobStorageConnectionString, queueName,
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                });

            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }
    }
}
