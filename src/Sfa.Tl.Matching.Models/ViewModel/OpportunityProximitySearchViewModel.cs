namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class OpportunityProximitySearchViewModel
    {
        public SearchParametersViewModel SearchParameters { get; set; }
        public OpportunityProximitySearchResultsViewModel SearchResults { get; set; }
        public bool IsValidSearch { get; set; } = true;
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
    }
}