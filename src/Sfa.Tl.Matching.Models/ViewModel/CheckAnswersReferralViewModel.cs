using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersReferralViewModel
    {
        public int OpportunityId { get; set; }
        public string Postcode { get; set; }
        public CheckAnswersPlacementViewModel PlacementInformation { get; set; }

        public List<ReferralsViewModel> Providers { get; set; }
    }
}