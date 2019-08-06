namespace Sfa.Tl.Matching.Models.Command
{
    public class SendEmployerReferralEmail
    {
        public int OpportunityId { get; set; }
        public int BackgroundProcessHistoryId { get; set; }
    }
}