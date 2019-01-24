using Sfa.Tl.Matching.Core.Enums;

namespace Sfa.Tl.Matching.Infrastructure.Blob
{
    public class BlobData
    {
        public string Name { get; }
        public FileUploadType Type { get; }
        public string ContentType { get; }
        public byte[] Data { get; }

        public string FileName => $"{Type.ToString()}/{Name}";

        public BlobData(string name, FileUploadType type, string contentType, byte[] data)
        {
            Name = name;
            Type = type;
            ContentType = contentType;
            Data = data;
        }
    }
}