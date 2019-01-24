using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Infrastructure.Configuration;

namespace Sfa.Tl.Matching.Infrastructure.Blob
{
    public class BlobService : IBlobService
    {
        private readonly MatchingConfiguration _configuration;

        public BlobService(MatchingConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Upload(BlobData blobData)
        {
            var blobContainer = await GetContainer(ContainerConstants.Files);
            var blockBlob = blobContainer.GetBlockBlobReference(blobData.FileName);
            blockBlob.Properties.ContentType = blobData.ContentType;

            await blockBlob.UploadFromByteArrayAsync(blobData.Data, 0, blobData.Data.Length);
        }

        #region Private Methods
        private async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(_configuration.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudBlobClient();

            var blobContainer = client.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            return blobContainer;
        }
        #endregion
    }
}