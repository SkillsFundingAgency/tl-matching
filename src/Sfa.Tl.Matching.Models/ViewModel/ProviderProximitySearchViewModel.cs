namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximitySearchViewModel
    {
        public ProviderProximitySearchParametersViewModel SearchParameters { get; }
        public ProviderProximitySearchResultsViewModel SearchResults { get; set; }

        public ProviderProximitySearchViewModel(ProviderProximitySearchParametersViewModel searchParameters)
        {
            SearchParameters = searchParameters;
        }
    }
}