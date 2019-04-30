﻿using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MessageQueue
{
    public class When_MessageQueueService_Is_Called_To_Push_New_Proximity_Data_Message
    {
        private readonly MessageQueueService _messageQueueService;
        private readonly CloudQueue _queue;

        public When_MessageQueueService_Is_Called_To_Push_New_Proximity_Data_Message()
        {
            var storageAccount = CloudStorageAccount.Parse(TestConfiguration.MatchingConfiguration.BlobStorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(QueueName.GetProximityQueue);
            _messageQueueService = new MessageQueueService(new NullLogger<MessageQueueService>(), TestConfiguration.MatchingConfiguration);
        }

        [Fact]
        public async Task Then_Message_Is_Queued()
        {
            CloudQueueMessage retrievedMessage = null;
            try
            {
                await _messageQueueService.PushProximityDataAsync(new GetProximityData { Postcode = "CV12WT", ProviderVenueId = 123 });
                retrievedMessage = await _queue.GetMessageAsync();
                retrievedMessage.Should().NotBeNull();
                retrievedMessage.AsString.Should().Contain("CV12WT");
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