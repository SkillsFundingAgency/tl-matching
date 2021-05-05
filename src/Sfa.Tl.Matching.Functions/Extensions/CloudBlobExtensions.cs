using Microsoft.Azure.Storage.Blob;
using Sfa.Tl.Matching.Application.Configuration;

namespace Sfa.Tl.Matching.Functions.Extensions
{
    public static class CloudBlobExtensions
    {
        public static string GetCreatedByMetadata(this ICloudBlob cloudBlob)
        {
            var createdBy = cloudBlob.Metadata.ContainsKey(Constants.MetadataKeyCreatedBy)
                ? cloudBlob.Metadata[Constants.MetadataKeyCreatedBy]
                : string.Empty;

            return createdBy;
        }
    }
}
