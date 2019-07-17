using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityPipelineDto
    {
        public string CompanyName { get; set; }

        public IList<ReferralItemDto> ReferralItems { get; set; }
        public IList<ProvisionGapItemDto> ProvisionGapItems { get; set; }
    }
}