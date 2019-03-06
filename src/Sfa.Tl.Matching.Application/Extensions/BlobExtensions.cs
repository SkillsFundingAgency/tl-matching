using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class BlobExtensions
    {
        private const string MetadataKeyCreatedBy = "createdBy";

        public static void AddCreatedByMetadata(this ICloudBlob cloudBlob, string createdBy)
        {
            cloudBlob.Metadata[MetadataKeyCreatedBy] = createdBy;
        }

        public static string GetCreatedByMetadata(this ICloudBlob cloudBlob)
        {
            var createdBy = cloudBlob.Metadata.ContainsKey(MetadataKeyCreatedBy)
                ? cloudBlob.Metadata[MetadataKeyCreatedBy]
                : string.Empty;

            return createdBy;
        }
    }
}