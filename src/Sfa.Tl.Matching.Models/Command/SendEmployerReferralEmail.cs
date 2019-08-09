using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Command
{
    public class SendEmployerReferralEmail
    {
        public int OpportunityId { get; set; }
        public IEnumerable<int> ItemIds { get; set; }
        public int BackgroundProcessHistoryId { get; set; }
    }
}