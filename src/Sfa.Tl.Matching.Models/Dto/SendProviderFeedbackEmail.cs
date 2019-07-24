using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class SendProviderFeedbackEmail
    {
        public int BackgroundProcessHistoryId { get; set; }
    }

    public class SendProviderReferralEmail
    {
        public int BackgroundProcessHistoryId { get; set; }
        public IEnumerable<int> OpportunityItemIds { get; set; }
        public int OpportunityId { get; set; }

    }

    public class SendEmployerReferralEmail
    {
        public int OpportunityId { get; set; }
        public IEnumerable<int> OpportunityItemIds { get; set; }
        public int BackgroundProcessHistoryId { get; set; }

    }

}