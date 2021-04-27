using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Sfa.Tl.Matching.Application.Constants;
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
            var blobClient = await GetBlobClient(
                dto.ImportType.ToString().ToLowerInvariant(),
                dto.FileName);
           
            await using var uploadFileStream = new MemoryStream(dto.Data);

            var metadata = new Dictionary<string, string> 
            {
                { ApplicationConstants.CreatedByMetadataKey, dto.UserName }
            };

            await blobClient.UploadAsync(uploadFileStream, //overwrite:true
                metadata: metadata,
                httpHeaders: new BlobHttpHeaders { ContentType = dto.ContentType });

            uploadFileStream.Close();

            _logger.LogInformation($"Successfully uploaded {dto.FileName} to {dto.ImportType} folder");
        }
        
        private async Task<BlobClient> GetBlobClient(
            string containerName, 
            string fileName)
        {
            var blobContainerClient = await GetContainerAsync(containerName);

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            return blobClient;
        }

        private async Task<BlobContainerClient> GetContainerAsync(string containerName)
        {
            var blobServiceClient = new BlobServiceClient(_configuration.BlobStorageConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            return containerClient;
        }
    }
}