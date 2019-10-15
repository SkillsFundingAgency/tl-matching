using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MessageQueue
{
    public class When_MessageQueueService_Is_Called_To_Push_Failed_Email_Queue
    {
        private readonly MessageQueueService _messageQueueService;
        private readonly CloudQueue _queue;

        public When_MessageQueueService_Is_Called_To_Push_Failed_Email_Queue()
        {
            var storageAccount = CloudStorageAccount.Parse(TestConfiguration.MatchingConfiguration.BlobStorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(QueueName.FailedEmailQueue);
            _messageQueueService = new MessageQueueService(new NullLogger<MessageQueueService>(), TestConfiguration.MatchingConfiguration);

        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            CloudQueueMessage retrievedMessage = null;
            try
            {
                var notificationId = new Guid();
                await _messageQueueService.PushFailedEmailMessageAsync(new SendFailedEmail
                {
                    NotificationId = notificationId
                });

                retrievedMessage = await _queue.GetMessageAsync();
                retrievedMessage.Should().NotBeNull();
                retrievedMessage.As<Guid>().Should().Be(notificationId);
            }
            finally
            {
                if (retrievedMessage != null)
                {
                    await _queue.DeleteMessageAsync(retrievedMessage);
                }
            }
        }

    }
}
