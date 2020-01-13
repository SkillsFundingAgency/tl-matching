using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class PotentialOpportunityItems
    {
        public int EmailHistoryId { get; set; }
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string SentTo { get; set; }
        public string ContactEmail { get; set; }
        public DateTime EmailHistoryCreated{ get; set; }
        public DateTime? EmailHistoryModified { get; set; }
        public DateTime OpportunityItemCreated { get; set; }
        public DateTime? OpportunityItemModified { get; set; }
        public DateTime ProviderCreated { get; set; }
        public DateTime? ProviderModified { get; set; }
    }
}