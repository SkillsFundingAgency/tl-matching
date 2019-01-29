using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class DataImportDto
    {
        public Stream Data { get; set; }
        public bool HasHeaderRow { get; set; }
    }
}
