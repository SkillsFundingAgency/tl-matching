using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class DataBlobUploadService : IDataBlobUploadService
    {
        private readonly ILogger<DataBlobUploadService> _logger;
        private readonly MatchingConfiguration _configuration;

        public DataBlobUploadService(ILogger<DataBlobUploadService> logger, MatchingConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task UploadAsync(DataUploadDto dto)
        {
            var blockBlob = await GetBlockBlobReference(
                dto.ImportType.ToString().ToLowerInvariant(),
                dto.FileName, dto.ContentType, dto.UserName);

            await blockBlob.UploadFromByteArrayAsync(dto.Data, 0, dto.Data.Length);

            _logger.LogInformation($"Successfully uploaded {dto.FileName} to {dto.ImportType} folder");
        }

        public async Task UploadFromStreamAsync(DataStreamUploadDto dto)
        {
            var blockBlob = await GetBlockBlobReference(dto.ContainerName, dto.FileName, dto.ContentType, dto.UserName);

            await blockBlob.UploadFromStreamAsync(dto.DataStream);

            _logger.LogInformation($"Successfully uploaded {dto.FileName} to {dto.ContainerName} folder");
        }

        private async Task<CloudBlockBlob> GetBlockBlobReference(string containerName, string fileName,
            string contentType, string createdByUserName)
        {
            var blobContainer = await GetContainerAsync(containerName);

            var blockBlob = blobContainer.GetBlockBlobReference(fileName);
            blockBlob.AddCreatedByMetadata(createdByUserName);
            blockBlob.Properties.ContentType = contentType;
            return blockBlob;
        }

        private async Task<CloudBlobContainer> GetContainerAsync(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudBlobClient();

            var blobContainer = client.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }
    }
}