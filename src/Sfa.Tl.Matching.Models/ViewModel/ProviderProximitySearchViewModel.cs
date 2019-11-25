using System.Collections.Generic;
using System.Linq;

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

    public class ProviderProximitySearchResultsViewModel
    {
        public int SearchResultProviderCount => Results?.Count ?? 6;
        public IList<string> Results { get; set; } // TODO AU Fill in Results
    }

    public class ProviderProximitySearchParametersViewModel
    {
        public string Postcode { get; set; }
        public ProviderProximityFiltersViewModel[] Filters { get; set; }
        public List<string> SelectedFilters { get; } = new List<string>();

        public ProviderProximitySearchParametersViewModel()
        {

        }

        public ProviderProximitySearchParametersViewModel(string searchCriteria, IEnumerable<string> routes)
        {
            var criteria = searchCriteria.Split("-");
            Postcode = criteria[0];

            var filters = new List<ProviderProximityFiltersViewModel>();
            foreach (var route in routes)
            {
                var isSelected = false;
                for (var i = 1; i < criteria.Length; i++)
                {
                    if (!criteria[i].ToLower().Contains(route.ToLower())) continue;
                    isSelected = true;
                    break;
                }

                filters.Add(new ProviderProximityFiltersViewModel
                {
                    Name = route,
                    IsSelected = isSelected
                });
            }

            Filters = filters.ToArray();
            SelectedFilters = filters.Where(f => f.IsSelected).Select(f => f.Name).ToList();
        }
    }

    public class ProviderProximityFiltersViewModel
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
    }
}