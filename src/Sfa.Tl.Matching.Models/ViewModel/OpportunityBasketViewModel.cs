using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class OpportunityBasketViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameAka { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(CompanyNameAka) ?
            $"{CompanyName} ({CompanyNameAka})" : CompanyName;

        public int ReferralCount => ReferralItems?.Count ?? 0;

        public int ProvisionGapCount => ProvisionGapItems?.Count ?? 0;
        
        public IList<BasketReferralItemViewModel> ReferralItems { get; set; }
        public IList<BasketProvisionGapItemViewModel> ProvisionGapItems { get; set; }
        public OpportunityBasketType Type { get; set; }
    }
}