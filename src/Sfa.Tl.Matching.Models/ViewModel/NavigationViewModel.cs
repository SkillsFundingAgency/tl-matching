﻿namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class NavigationViewModel
    {
        public int OpportunityId { get; set; }
        public OpportunityBasketViewModel OpportunityBasket { get; set; }
        public string RouteName { get; set; }
        public string CancelText { get; set; }
    }
}
