using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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

                // Enumerate the blob's metadata.
                foreach (var metadataItem in properties.Value.Metadata)
                {
                    Debug.WriteLine($"\tKey: {metadataItem.Key}");
                    Debug.WriteLine($"\tValue: {metadataItem.Value}");
                }

                var createdBy = properties.Value.Metadata.ContainsKey(ApplicationConstants.CreatedByMetadataKey)
                    ? properties.Value.Metadata[ApplicationConstants.CreatedByMetadataKey]
                    : string.Empty;

                return createdBy;
            }
            catch (RequestFailedException e)
            {
                Debug.WriteLine($"HTTP error code {e.Status}: {e.ErrorCode}");
                return null;
            }
        }
    }
}