namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class FindEmployerViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public int SelectedEmployerId { get; set; }
        public string CompanyName { get; set; }
        public string PreviousCompanyName { get; set; }
    }
}