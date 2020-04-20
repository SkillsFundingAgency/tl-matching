using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class DataStreamUploadDto
    {
        public Stream DataStream { get; set; }
        public string ContentType { get; set; }
        public string ContainerName { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
    }
}