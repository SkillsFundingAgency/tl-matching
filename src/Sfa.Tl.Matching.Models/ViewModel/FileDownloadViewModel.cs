namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class FileDownloadViewModel
    {
        public string ContentType{ get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}