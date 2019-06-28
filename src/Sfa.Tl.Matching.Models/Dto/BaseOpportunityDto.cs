using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class BaseOpportunityDto
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}