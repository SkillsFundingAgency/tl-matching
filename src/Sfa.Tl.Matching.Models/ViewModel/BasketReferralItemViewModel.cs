using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class BasketReferralItemViewModel : OpportunityBasketItemViewModel
    {
        public List<ProviderNameViewModel> ProviderNames { get; set; }
        public string ProvidersDisplayText => 
            string.Join(", ", ProviderNames
                .Where(pn => pn.Postcode != null)
                .Select(pn => pn.ProviderName));
    }
}