namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderSearchParametersDto
    {
        public string Postcode { get; set; }

        public int SearchRadius { get; set; }

        public int? SelectedRouteId { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }
    }
}