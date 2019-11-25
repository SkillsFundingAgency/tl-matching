using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximitySearchViewModel
    {
        public ProviderProximitySearchParametersViewModel SearchParameters { get; }

        public ProviderProximitySearchViewModel(ProviderProximitySearchParametersViewModel searchParameters)
        {
            SearchParameters = searchParameters;
        }
    }

    public class ProviderProximitySearchParametersViewModel
    {
        public string Postcode { get; set; }
        public ProviderProximityFiltersViewModel[] Filters { get; set; }
        public bool HasFilters { get; set; }

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
                    HasFilters = true;
                    break;
                }

                filters.Add(new ProviderProximityFiltersViewModel
                {
                    Name = route,
                    IsSelected = isSelected
                });
            }

            Filters = filters.ToArray();
        }
    }

    public class ProviderProximityFiltersViewModel
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
    }
}