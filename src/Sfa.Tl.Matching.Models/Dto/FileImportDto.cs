using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class FileImportDto
    {
        public virtual Stream FileDataStream { get; set; }
        public virtual int?  NumberOfHeaderRows { get; set; }
        public string CreatedBy { get; set; }
    }
}