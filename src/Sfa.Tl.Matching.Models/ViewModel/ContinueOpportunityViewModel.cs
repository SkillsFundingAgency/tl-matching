using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ContinueOpportunityViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string SubmitAction { get; set; }
        public List<SelectedOpportunityItemViewModel> SelectedOpportunity { get; init; } = new();

        public bool IsReferralSelected
        {
            get
            {
                var referrals = SelectedOpportunity.Where(o => o.OpportunityType == OpportunityType.Referral.ToString()).ToList();

                return referrals.Count > 0 && referrals.Any(p => p.IsSelected);
            }
        }
    }
}