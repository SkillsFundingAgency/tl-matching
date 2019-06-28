namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ContinueOpportunityViewModel
    {
        public int OpportunityId { get; set; }
        public string SubmitAction { get; set; }
        public SelectedOpportunityItemViewModel[] SelectedOpportunity { get; set; }
    }
}