namespace Sfa.Tl.Matching.Infrastructure.Blob
{
    public class BlobData
    {
        public string Name { get; }
        public int Type { get; }
        public string ContentType { get; }
        public byte[] Data { get; }

        public string FileName => $"{Type.ToString()}/{Name}";

        public BlobData(string name, int type, string contentType, byte[] data)
        {
            Name = name;
            Type = type;
            ContentType = contentType;
            Data = data;
        }
    }
}