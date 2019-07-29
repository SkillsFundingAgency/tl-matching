namespace Sfa.Tl.Matching.Models.Dto
{
    public class FileDownloadDto
    {
        public string ContentType{ get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}