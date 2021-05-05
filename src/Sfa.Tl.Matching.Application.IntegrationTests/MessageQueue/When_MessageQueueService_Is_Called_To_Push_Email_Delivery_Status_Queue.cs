using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MessageQueue
{
    public class When_MessageQueueService_Is_Called_To_Push_Email_Delivery_Status_Queue
    {
        private readonly MessageQueueService _messageQueueService;
        private readonly QueueClient _queueClient;

        public When_MessageQueueService_Is_Called_To_Push_Email_Delivery_Status_Queue()
        {
            _queueClient = new QueueClient(
                TestConfiguration.MatchingConfiguration.BlobStorageConnectionString,
                QueueName.EmailDeliveryStatusQueue);

            _messageQueueService = new MessageQueueService(new NullLogger<MessageQueueService>(), TestConfiguration.MatchingConfiguration);
        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            QueueMessage retrievedMessage = null;
            try
            {
                var notificationId = new Guid();
                await _messageQueueService.PushEmailDeliveryStatusMessageAsync(new SendEmailDeliveryStatus
                {
                    NotificationId = notificationId
                });

                retrievedMessage = await _queueClient.ReceiveMessageAsync();
                retrievedMessage.Should().NotBeNull();
                retrievedMessage.As<Guid>().Should().Be(notificationId);
            }
            finally
            {
                if (retrievedMessage != null)
                {
                    await _queueClient.DeleteMessageAsync(retrievedMessage.MessageId, retrievedMessage.PopReceipt);
                }
            }
        }
    }
}
