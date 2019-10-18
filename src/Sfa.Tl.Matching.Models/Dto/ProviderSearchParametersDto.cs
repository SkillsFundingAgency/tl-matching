namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderSearchParametersDto
    {
        public string Postcode { get; set; }

        public int AlternativeRoutesSearchRadius { get; set; }

        public int? SelectedRouteId { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }
    }
}