using System;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Sfa.Tl.Matching.Application.Constants;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class BlobExtensions
    {
        public static async Task<string> GetCreatedByMetadata(this BlobClient blobClient)
        {
            try
            {
                var properties = await blobClient.GetPropertiesAsync();

                var createdBy = properties.Value.Metadata.ContainsKey(ApplicationConstants.CreatedByMetadataKey)
                    ? properties.Value.Metadata[ApplicationConstants.CreatedByMetadataKey]
                    : string.Empty;

                return createdBy;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                return null;
            }
        }
    }
}