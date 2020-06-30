using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
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
        private readonly CloudQueue _queue;

        public When_MessageQueueService_Is_Called_To_Push_New_Provider_Quarterly_Request_Message()
        {
            var storageAccount = CloudStorageAccount.Parse(TestConfiguration.MatchingConfiguration.BlobStorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(QueueName.ProviderQuarterlyRequestQueue);
            _messageQueueService = new MessageQueueService(new NullLogger<MessageQueueService>(), TestConfiguration.MatchingConfiguration);
        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            CloudQueueMessage retrievedMessage = null;
            try
            {
                await _messageQueueService.PushProviderQuarterlyRequestMessageAsync(new SendProviderQuarterlyUpdateEmail
                    {BackgroundProcessHistoryId = 1001});
                retrievedMessage = await _queue.GetMessageAsync();
                retrievedMessage.Should().NotBeNull();
                retrievedMessage.AsString.Should().Contain("1001");
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
