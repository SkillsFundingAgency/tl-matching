using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;
    }
}