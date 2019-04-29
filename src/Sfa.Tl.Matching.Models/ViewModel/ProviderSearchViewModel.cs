namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderSearchViewModel
    {
        public bool IsAuthorisedUser { get; set; }

        public ProviderSearchParametersViewModel SearchParameters { get; }
        public ProviderSearchResultsViewModel SearchResults { get; set; }

        public ProviderSearchViewModel(ProviderSearchParametersViewModel searchParameters)
        {
            SearchParameters = searchParameters;
        }
    }
}