using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderProximitySearchParametersDto
    {
        public string Postcode { get; set; }

        public int SearchRadius { get; set; }

        public IList<int> SelectedRoutes { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}