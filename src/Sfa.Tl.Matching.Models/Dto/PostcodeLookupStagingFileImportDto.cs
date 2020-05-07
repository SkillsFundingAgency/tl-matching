using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PostcodeLookupStagingFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        //Use the "pcds" column from the input - this is outward code + space + inward code
        [Column(Order = 2)] public string Postcode { get; set; }
        [Column(Order = 44)] public string PrimaryLepCode { get; set; }
        [Column(Order = 45)] public string SecondaryLepCode { get; set; }
    }
}