using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmailHistoryWithPotentialOpportunityItems
    {
        public int EmailHistoryId { get; set; }
        public int OpportunityId { get; set; }
        public IEnumerable<PotentialOpportunityItems> PotentialOpportunityItems { get; set; }
    }
}