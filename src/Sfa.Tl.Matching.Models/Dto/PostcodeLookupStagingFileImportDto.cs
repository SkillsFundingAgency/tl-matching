using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PostcodeLookupStagingFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string Postcode { get; set; }
        [Column(Order = 44)] public string LepCode { get; set; }
    }
}