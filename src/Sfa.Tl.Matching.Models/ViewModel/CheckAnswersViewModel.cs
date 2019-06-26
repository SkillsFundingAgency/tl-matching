using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string Postcode { get; set; }
        public CheckAnswersPlacementViewModel PlacementInformation { get; set; }
        public List<ReferralsViewModel> Providers { get; set; }
    }
}