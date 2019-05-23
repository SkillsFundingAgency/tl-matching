using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class LearningAimsReferenceStagingFileImportDto : FileImportDto
    {
        public override Stream FileDataStream { get; set; }
        public override int? NumberOfHeaderRows => 1;

        [Column(Order = 0)] public string LearnAimRef { get; set; }
        [Column(Order = 3)] public string LearnAimRefTitle { get; set; }
        [Column(Order = 7)] public string AwardOrgAimRef { get; set; }
        [Column(Order = 55)] public string SourceCreatedOn { get; set; }
        [Column(Order = 57)] public string SourceModifiedOn { get; set; }
    }
}