namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CreateOpportunityViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string Postcode { get; set; }

        public int? SelectedRouteId { get; set; }
        public int? SearchResultProviderCount { get; set; }
        public string CompanyNameWithAka { get; set; }
    }
}