namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SearchViewModel
    {
        public SearchParametersViewModel SearchParameters { get; set; }
        public SearchResultsViewModel SearchResults { get; set; }
        public bool IsValidSearch { get; set; } = true;
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
    }
}