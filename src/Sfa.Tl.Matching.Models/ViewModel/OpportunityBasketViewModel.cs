using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class OpportunityBasketViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public IList<OpportunityBasketItemViewModel> Items { get; set; }
        public OpportunityBasketType Type { get; set; }
    }
}