using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MessageQueue
{
    public class When_MessageQueueService_Is_Called_To_Push_New_Provider_Quarterly_Request_Message : IClassFixture<MessageQueueServiceFixture>
    {
        private readonly MessageQueueServiceFixture _fixture;

        public When_MessageQueueService_Is_Called_To_Push_New_Provider_Quarterly_Request_Message(MessageQueueServiceFixture fixture)
        {
            _fixture = fixture;
            _fixture.GetMessageQueueServiceWithQueue(QueueName.ProviderQuarterlyRequestQueue);
        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            CloudQueueMessage retrievedMessage = null;
            try
            {
                await _fixture.MessageQueueService.PushProviderQuarterlyRequestMessageAsync(new SendProviderQuarterlyUpdateEmail { BackgroundProcessHistoryId = 1001 });
                retrievedMessage = await _fixture.Queue.GetMessageAsync();
                retrievedMessage.Should().NotBeNull();
                retrievedMessage.AsString.Should().Contain("1001");
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
