namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderSearchViewModel
    {
        public ProviderSearchParametersViewModel SearchParameters { get; }
        public ProviderSearchResultsViewModel SearchResults { get; set; } = new ProviderSearchResultsViewModel();

        public ProviderSearchViewModel(ProviderSearchParametersViewModel searchParameters)
        {
            SearchParameters = searchParameters;
        }
    }
}