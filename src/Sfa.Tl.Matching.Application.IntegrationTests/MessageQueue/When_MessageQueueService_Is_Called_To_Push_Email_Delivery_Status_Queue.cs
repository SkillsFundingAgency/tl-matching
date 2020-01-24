using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MessageQueue
{
    public class When_MessageQueueService_Is_Called_To_Push_Email_Delivery_Status_Queue :IClassFixture<MessageQueueServiceFixture>
    {
        private readonly MessageQueueServiceFixture _fixture;

        public When_MessageQueueService_Is_Called_To_Push_Email_Delivery_Status_Queue(MessageQueueServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.GetMessageQueueServiceWithQueue(QueueName.EmailDeliveryStatusQueue);
        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            CloudQueueMessage retrievedMessage = null;
            try
            {
                var notificationId = new Guid();
                await _fixture.MessageQueueService.PushEmailDeliveryStatusMessageAsync(new SendEmailDeliveryStatus
                {
                    NotificationId = notificationId
                });

                retrievedMessage = await _fixture.Queue.GetMessageAsync();
                retrievedMessage.Should().NotBeNull();
                retrievedMessage.As<Guid>().Should().Be(notificationId);
            }
            finally
            {
                if (retrievedMessage != null)
                {
                    await _fixture.Queue.DeleteMessageAsync(retrievedMessage);
                }
            }
        }

    }
}
