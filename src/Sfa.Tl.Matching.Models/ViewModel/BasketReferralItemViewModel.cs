using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class BasketReferralItemViewModel : OpportunityBasketItemViewModel
    {
        public List<ProviderNameViewModel> ProviderNames { get; set; }
        public string ProvidersDisplayText => string.Join(", ", ProviderNames.Select(pn => pn.ProviderName));
    }
}