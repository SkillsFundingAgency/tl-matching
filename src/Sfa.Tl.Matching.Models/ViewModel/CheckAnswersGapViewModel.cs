namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersGapViewModel
    {
        public int OpportunityId { get; set; }
        public bool ConfirmationSelected { get; set; }
        public string CreatedBy { get; set; }
        public CheckAnswersPlacementViewModel PlacementInformation { get; set; }
    }
}