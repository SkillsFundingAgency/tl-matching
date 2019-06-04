// ReSharper disable UnusedMember.Global
namespace Sfa.Tl.Matching.Models.Dto
{
    // ReSharper disable once UnusedMember.Global
    public class EmailHistoryDto
    {
        public int? OpportunityId { get; set; }
        public int EmailTemplateId { get; set; }
        public string SentTo { get; set; }
        public string CopiedTo { get; set; }
        public string BlindCopiedTo { get; set; }
        public string CreatedBy { get; set; }
    }
}
