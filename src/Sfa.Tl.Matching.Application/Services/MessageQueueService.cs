using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly ILogger<DataBlobUploadService> _logger;
        private readonly MatchingConfiguration _configuration;


        public MessageQueueService(ILogger<DataBlobUploadService> logger, MatchingConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Push(GetProximityData getProximityData)
        {
            var message = JsonConvert.SerializeObject(getProximityData);
            var queue = await GetProximityQueue();
            await queue.AddMessageAsync(new CloudQueueMessage(message));
        }

        private async Task<CloudQueue> GetProximityQueue()
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudQueueClient();

            var queueReference = client.GetQueueReference(QueueName.GetProximityQueue);
            await queueReference.CreateIfNotExistsAsync();

            return queueReference;
        }
    }
}
