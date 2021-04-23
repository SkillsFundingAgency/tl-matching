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
    public class When_MessageQueueService_Is_Called_To_Push_New_Provider_Quarterly_Request_Message
    {
        private readonly MessageQueueService _messageQueueService;
        private readonly QueueClient _queueClient;

        public When_MessageQueueService_Is_Called_To_Push_New_Provider_Quarterly_Request_Message()
        {
            _queueClient = new QueueClient(
                TestConfiguration.MatchingConfiguration.BlobStorageConnectionString,
                QueueName.ProviderQuarterlyRequestQueue);

            _messageQueueService = new MessageQueueService(new NullLogger<MessageQueueService>(), TestConfiguration.MatchingConfiguration);
        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            QueueMessage retrievedMessage = null;

            try
            {
                await _messageQueueService.PushProviderQuarterlyRequestMessageAsync(new SendProviderQuarterlyUpdateEmail
                { BackgroundProcessHistoryId = 1001 });
                retrievedMessage = await _queueClient.ReceiveMessageAsync();

                retrievedMessage.Should().NotBeNull();
                retrievedMessage.MessageText.Should().Contain("1001");
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
