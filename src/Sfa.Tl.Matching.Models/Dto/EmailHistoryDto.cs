namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmailHistoryDto
    {
        public int OpportunityId { get; set; }
        public int EmailTemplateId { get; set; }
        public string SentTo { get; set; }
        public string CopiedTo { get; set; }
        public string BlindCopiedTo { get; set; }
    }
}
