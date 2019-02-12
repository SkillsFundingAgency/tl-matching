using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Tl.Matching.Infrastructure.Extensions
{
    public static class BlobExtensions
    {
        private const string MetadataKeyCreatedBy = "createdBy";

        public static void AddCreatedByMetadata(this CloudBlob cloudBlob, string createdBy)
        {
            cloudBlob.Metadata[MetadataKeyCreatedBy] = createdBy;
        }

        public static string GetCreatedByMetadata(this CloudBlob cloudBlob)
        {
            var createdBy = cloudBlob.Metadata.ContainsKey(MetadataKeyCreatedBy)
                ? cloudBlob.Metadata[MetadataKeyCreatedBy]
                : string.Empty;

            return createdBy;
        }
    }
}