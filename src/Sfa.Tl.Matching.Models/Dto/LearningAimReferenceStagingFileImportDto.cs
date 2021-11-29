using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class LearningAimReferenceStagingFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string LarId { get; set; }
        [Column(Order = 3)] public string Title { get; set; }
        [Column(Order = 7)] public string AwardOrgLarId { get; set; }
        [Column(Order = 56)] public string SourceCreatedOn { get; set; }
        [Column(Order = 58)] public string SourceModifiedOn { get; set; }
    }
}