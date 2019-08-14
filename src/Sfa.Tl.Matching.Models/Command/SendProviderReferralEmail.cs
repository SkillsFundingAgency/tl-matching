using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Command
{
    public class SendProviderReferralEmail
    {
        public int BackgroundProcessHistoryId { get; set; }
        public int OpportunityId { get; set; }
        public IEnumerable<int> ItemIds { get; set; }
    }
}