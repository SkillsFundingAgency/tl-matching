using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximitySearchParametersViewModel
    {
        public string Postcode { get; set; }
        public ProviderProximityFiltersViewModel[] Filters { get; }
        public List<string> SelectedFilters { get; } = new List<string>();

        public ProviderProximitySearchParametersViewModel() { }

        public ProviderProximitySearchParametersViewModel(string searchCriteria, IEnumerable<string> routes)
        {
            var criteria = searchCriteria.Split("-");
            Postcode = criteria[0];

            Filters = PopulateFilters(criteria, routes);
            SelectedFilters = Filters.Where(f => f.IsSelected).Select(f => f.Name).ToList();
        }

        private static ProviderProximityFiltersViewModel[] PopulateFilters(IReadOnlyList<string> criteria, IEnumerable<string> routes)
        {
            var filters = new List<ProviderProximityFiltersViewModel>();
            foreach (var route in routes)
            {
                var isSelected = false;
                for (var i = 1; i < criteria.Count; i++)
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

            return filters.ToArray();
        }
    }
}