using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityReportDto
    {
        public string CompanyName { get; set; }

        public IList<ReferralItemDto> ReferralItems { get; set; }
        public IList<ProvisionGapItemDto> ProvisionGapItems { get; set; }
    }
}