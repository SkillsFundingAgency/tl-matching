namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersProvisionGapViewModel
    {
        public int OpportunityId { get; set; }
        public bool ConfirmationSelected { get; set; }
        public string CreatedBy { get; set; }
        public CheckAnswersPlacementViewModel PlacementInformation { get; set; }
    }
}