using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ContinueOpportunityViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string SubmitAction { get; set; }
        public List<SelectedOpportunityItemViewModel> SelectedOpportunity { get; set; } = new List<SelectedOpportunityItemViewModel>();
    }
}