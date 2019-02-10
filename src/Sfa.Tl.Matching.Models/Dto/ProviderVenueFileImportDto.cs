using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        public int ProviderId { get; set; }

        [Column(Order = 0)] public string UkPrn { get; set; }
        [Column(Order = 1)] public string PostCode { get; set; }
        [Column(Order = 3)] public string Source { get; set; }
        [Column(Order = 3)] public string Status { get; set; }
        [Column(Order = 4)] public string StatusReason { get; set; }
    }
}