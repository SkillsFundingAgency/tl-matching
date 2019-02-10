using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string UkPrn { get; set; }
        [Column(Order = 1)] public string ProviderName { get; set; }
        [Column(Order = 2)] public string OfstedRating { get; set; }
        [Column(Order = 3)] public string Status { get; set; }
        [Column(Order = 4)] public string StatusReason { get; set; }
        [Column(Order = 5)] public string PrimaryContactName { get; set; }
        [Column(Order = 6)] public string PrimaryContactEmail { get; set; }
        [Column(Order = 7)] public string PrimaryContactTelephone { get; set; }
        [Column(Order = 8)] public string SecondaryContactName { get; set; }
        [Column(Order = 9)] public string SecondaryContactEmail { get; set; }
        [Column(Order = 10)] public string SecondaryContactTelephone { get; set; }
        [Column(Order = 11)] public string Source { get; set; }
    }
}