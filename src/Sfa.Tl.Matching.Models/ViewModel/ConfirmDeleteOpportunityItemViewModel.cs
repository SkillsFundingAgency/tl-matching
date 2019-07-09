namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ConfirmDeleteOpportunityItemViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameAka { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(CompanyNameAka) ?
            $"{CompanyName} ({CompanyNameAka})" : CompanyName;
        public string Postcode { get; set; }
        public string JobRole { get; set; }
        public int BasketItemCount { get; set; }
        public int? Placements { get; set; }

        public string PlacementsDetail => (Placements.GetValueOrDefault() > 1) ? Placements.ToString() : "at least 1";
        public string DeleteWarningText => BasketItemCount == 1 ? "This cannot be undone and will mean this employer has no more saved opportunities." : "This cannot be undone.";
        public string SubmitActionText => BasketItemCount == 1 ? "Confirm and finish" : "Confirm and continue";
    }
}