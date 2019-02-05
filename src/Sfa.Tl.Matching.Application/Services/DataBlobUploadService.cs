using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Configuration;
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

        public async Task Upload(DataImportDto dto)
        {
            var blobContainer = await GetContainer(dto.ImportType.ToString().ToLowerInvariant());

            var blockBlob = blobContainer.GetBlockBlobReference(dto.FileName);

            blockBlob.Properties.ContentType = dto.ContentType;

            await blockBlob.UploadFromByteArrayAsync(dto.Data, 0, dto.Data.Length);

            _logger.LogInformation($"successfuly uploaded {dto.FileName} to {dto.ImportType} folder");
        }

        private async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudBlobClient();

            var blobContainer = client.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }
    }
}