using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersReferralViewModel
    {
        public int OpportunityId { get; set; }
        public CheckAnswersPlacementViewModel PlacementInformation { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm that we can share the employer’s details with the selected providers.")]
        public bool ConfirmationSelected { get; set; }
        public List<ReferralsViewModel> Providers { get; set; }
    }
}