using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class BaseOpportunityUpdateDto
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}