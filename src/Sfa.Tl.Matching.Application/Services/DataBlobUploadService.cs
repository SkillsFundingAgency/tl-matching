using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class DataBlobUploadService : IDataBlobUploadService
    {
        public const string Files = "files";

        // ReSharper disable once NotAccessedField.Local
        private readonly ILogger<DataBlobUploadService> _logger;
        private readonly MatchingConfiguration _configuration;

        public DataBlobUploadService(ILogger<DataBlobUploadService> logger, MatchingConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<CloudBlockBlob> Upload(SelectedImportDataViewModel viewModel)
        {
            var blobContainer = await GetContainer(Files);
            var blockBlob = blobContainer.GetBlockBlobReference(viewModel.FileName);
            blockBlob.Properties.ContentType = viewModel.ContentType;

            await blockBlob.UploadFromByteArrayAsync(viewModel.Data, 0, viewModel.Data.Length);

            return blockBlob;
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